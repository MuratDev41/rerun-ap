using System;
using System.Linq;
using BepInEx;
using HarmonyLib;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using UnityEngine;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;

namespace RerunArchipelago
{
    [BepInPlugin("com.archipelago.rerun", "RE:RUN Archipelago", "1.0.0")]
    public class ArchipelagoPlugin : BaseUnityPlugin
    {
        public static ArchipelagoPlugin Instance;
        public ArchipelagoSession Session;
        public new BepInEx.Logging.ManualLogSource Logger;

        // DeathLink
        private DeathLinkService deathLinkService;
        private int deathLinkAmnesty = 0;
        private int deathCounter = 0;
        private bool isReceivingDeath = false;

        // Settings for Archipelago connection
        private string serverUrl = "archipelago.gg:38281";
        private string slotName = "Player1";
        private string password = "";

        // UI State
        private bool showMenu = true;
        private Rect windowRect = new Rect(20, 20, 320, 320);

        // Notifications
        private System.Collections.Generic.List<string> notifications = new System.Collections.Generic.List<string>();
        private float notificationTimer = 0f;

        // Powerup unlock flags (static so patches can read them)
        public static bool HasSword = false;
        public static bool HasDoubleJump = false;
        public static bool HasRewind = false;

        // Level unlock flags (sceneIndex based)
        public static bool[] UnlockedLevels = new bool[12];

        private void Awake()
        {
            Instance = this;
            Logger = base.Logger;
            Logger.LogInfo("Plugin RE:RUN Archipelago is loaded!");

            // Initialize unlocked levels (Menu and Level 0 always accessible)
            UnlockedLevels[0] = true; // Menu
            UnlockedLevels[1] = true; // Level 0

            var harmony = new Harmony("com.archipelago.rerun.patches");
            harmony.PatchAll();
        }

        private void Connect()
        {
            try
            {
                Session = ArchipelagoSessionFactory.CreateSession(serverUrl);

                var result = Session.TryConnectAndLogin("RE:RUN", slotName, ItemsHandlingFlags.AllItems, new Version(0, 6, 0), null, null, password);

                if (!result.Successful)
                {
                    LoginFailure failure = (LoginFailure)result;
                    string err = string.Join(", ", failure.Errors);
                    Logger.LogError($"Failed to connect: {err}");
                    AddNotification($"Connection failed: {err}");
                    return;
                }

                var loginSuccess = (LoginSuccessful)result;
                var slotData = loginSuccess.SlotData;

                Logger.LogInfo("Successfully connected to Archipelago!");
                AddNotification("Connected to Archipelago!");
                showMenu = false;

                // DeathLink Setup
                if (slotData.TryGetValue("death_link", out var dl) && dl.ToString().ToLower() == "true")
                {
                    deathLinkService = Session.CreateDeathLinkService();
                    deathLinkService.OnDeathLinkReceived += OnDeathLinkReceived;
                    deathLinkService.EnableDeathLink();
                    Logger.LogInfo("DeathLink enabled.");
                }

                if (slotData.TryGetValue("death_link_amnesty", out var am))
                {
                    int.TryParse(am.ToString(), out deathLinkAmnesty);
                    Logger.LogInfo($"DeathLink Amnesty set to {deathLinkAmnesty}");
                }

                // Listen for received items
                Session.Items.ItemReceived += OnItemReceived;

                // Process any items we already have
                while (Session.Items.Any())
                {
                    var item = Session.Items.DequeueItem();
                    ProcessItem(item);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception during Archipelago connect: {ex.Message}");
                AddNotification($"Error: {ex.Message}");
            }
        }

        private void OnItemReceived(ReceivedItemsHelper helper)
        {
            var item = helper.DequeueItem();
            ProcessItem(item);
        }

        private void ProcessItem(ItemInfo item)
        {
            Logger.LogInfo($"Received item: {item.ItemName} (ID: {item.ItemId})");
            AddNotification($"Received: {item.ItemName}");

            long baseId = 3310000;
            long itemId = item.ItemId;

            if (itemId == baseId + 301) 
            {
                HasSword = true;
            }
            else if (itemId == baseId + 302) 
            {
                HasDoubleJump = true;
            }
            else if (itemId == baseId + 303) 
            {
                HasRewind = true;
            }
            else if (itemId >= baseId + 101 && itemId <= baseId + 111)
            {
                int levelNum = (int)(itemId - (baseId + 101));
                UnlockedLevels[levelNum + 1] = true;
                Logger.LogInfo($"Unlocked Level {levelNum}");
            }
        }

        public void AddNotification(string message)
        {
            notifications.Add(message);
            if (notifications.Count > 5) notifications.RemoveAt(0);
            notificationTimer = 5f;
        }

        private void Update()
        {
            // Toggle menu with P
            if (Input.GetKeyDown(KeyCode.P))
                showMenu = !showMenu;

            // Notification timer
            if (notifications.Count > 0)
            {
                notificationTimer -= Time.deltaTime;
                if (notificationTimer <= 0)
                {
                    notifications.RemoveAt(0);
                    if (notifications.Count > 0) notificationTimer = 5f;
                }
            }

            if (showMenu)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void OnGUI()
        {
            // Draw Notifications (top-right)
            if (notifications.Count > 0)
            {
                GUIStyle style = new GUIStyle(GUI.skin.box);
                style.alignment = TextAnchor.UpperLeft;
                style.fontSize = 18;
                style.normal.textColor = UnityEngine.Color.white;
                string notifText = string.Join("\n", notifications);
                GUI.Box(new Rect(Screen.width - 330, 20, 310, 28 * notifications.Count + 10), notifText, style);
            }

            if (showMenu)
                windowRect = GUI.Window(0, windowRect, DrawMenu, "Archipelago Connection  [P]");
        }

        private void DrawMenu(int windowID)
        {
            GUILayout.Space(8);

            if (Session == null)
            {
                GUILayout.Label("Server URL:");
                serverUrl = GUILayout.TextField(serverUrl);

                GUILayout.Label("Slot Name:");
                slotName = GUILayout.TextField(slotName);

                GUILayout.Label("Password:");
                password = GUILayout.TextField(password, GUILayout.Width(200));

                GUILayout.Space(10);

                if (GUILayout.Button("Connect"))
                    Connect();
                
                GUILayout.Label("Status: ● Disconnected", GUILayout.ExpandWidth(true));
            }
            else
            {
                string status = _goalReached ? "★ GOAL REACHED ★" : $"● Connected as {slotName}";
                GUILayout.Label(status, GUILayout.ExpandWidth(true));
                
                GUILayout.Space(10);
                GUILayout.Label("<b>Powerups:</b>");
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Sword: {(HasSword ? "<color=green>✓</color>" : "<color=red>✗</color>")}");
                GUILayout.Label($"Jump: {(HasDoubleJump ? "<color=green>✓</color>" : "<color=red>✗</color>")}");
                GUILayout.Label($"Rewind: {(HasRewind ? "<color=green>✓</color>" : "<color=red>✗</color>")}");
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
                GUILayout.Label("<b>Level Keys:</b>");
                
                // Draw levels in rows of 4
                for (int row = 0; row < 3; row++)
                {
                    GUILayout.BeginHorizontal();
                    for (int col = 0; col < 4; col++)
                    {
                        int i = row * 4 + col;
                        if (i > 10) break;
                        string color = UnlockedLevels[i + 1] ? "green" : "red";
                        string mark = UnlockedLevels[i + 1] ? "✓" : "✗";
                        GUILayout.Label($"L{i}: <color={color}>{mark}</color>", GUILayout.Width(60));
                    }
                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(10);
                int found = _sentLocations.Count;
                GUILayout.Label($"Checks Found: {found} / 61"); // 11 levels + 5 powerups + 45 enemies
            }

            GUILayout.Space(10);
            if (GUILayout.Button("Close Menu [P]"))
                showMenu = false;

            GUI.DragWindow();
        }

        private static float _lastErrorTime = -10f;
        public static void LogError(Exception ex, string context)
        {
            // Rate limit to 1 error every 5 seconds to prevent log spamming/crashes
            if (UnityEngine.Time.time - _lastErrorTime < 5f) return;
            _lastErrorTime = UnityEngine.Time.time;

            string msg = $"[RE:RUN Archipelago] CRASH in {context}: {ex.Message}\n{ex.StackTrace}";
            if (Instance != null && Instance.Logger != null)
                Instance.Logger.LogError(msg);
            else
                UnityEngine.Debug.LogError(msg);
        }

        private System.Collections.Generic.HashSet<long> _sentLocations = new System.Collections.Generic.HashSet<long>();
        public void SendLocationCheck(long locationId, string label)
        {
            if (Session == null) return;
            if (_sentLocations.Contains(locationId)) return;

            try
            {
                Session.Locations.CompleteLocationChecks(locationId);
                _sentLocations.Add(locationId);
                Logger.LogInfo($"Sent check: {label} (ID: {locationId})");
                
                // After sending a check, see if we've reached the goal
                CheckGoalStatus();
            }
            catch (Exception ex)
            {
                LogError(ex, "SendLocationCheck");
            }
        }

        private bool _goalReached = false;
        public void CheckGoalStatus()
        {
            if (Session == null || _goalReached) return;

            // Check if all levels 0-10 (location IDs 3310001 to 3310011) have been checked
            bool allBeaten = true;
            for (int i = 1; i <= 11; i++)
            {
                long locId = 3310000 + i;
                if (!Session.Locations.AllLocationsChecked.Contains(locId) && !_sentLocations.Contains(locId))
                {
                    allBeaten = false;
                    break;
                }
            }

            if (allBeaten)
            {
                _goalReached = true;
                var statusPacket = new Archipelago.MultiClient.Net.Packets.StatusUpdatePacket();
                statusPacket.Status = ArchipelagoClientState.ClientGoal;
                Session.Socket.SendPacket(statusPacket);
                
                Logger.LogInfo("GOAL REACHED! All levels completed.");
                AddNotification("★ GOAL REACHED! ★");
            }
        }

        private void OnDeathLinkReceived(DeathLink deathLink)
        {
            try
            {
                Logger.LogInfo($"DeathLink received from {deathLink.Source}: {deathLink.Cause}");
                isReceivingDeath = true;
                
                // Find player and kill them
                var player = GameObject.FindObjectOfType<PlayerStatus>();
                if (player != null)
                {
                    AccessTools.Method(typeof(PlayerStatus), "Kill").Invoke(player, null);
                    AddNotification($"DeathLink: {deathLink.Source}");
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "OnDeathLinkReceived");
            }
            finally
            {
                isReceivingDeath = false;
            }
        }

        public void SendDeathLink()
        {
            if (deathLinkService == null || isReceivingDeath) return;

            deathCounter++;
            if (deathCounter <= deathLinkAmnesty)
            {
                Logger.LogInfo($"DeathLink Amnesty: {deathCounter}/{deathLinkAmnesty}");
                return;
            }

            deathCounter = 0;
            try
            {
                var deathLink = new DeathLink(slotName, $"{slotName} ran out of time.");
                deathLinkService.SendDeathLink(deathLink);
                Logger.LogInfo("DeathLink sent.");
            }
            catch (Exception ex)
            {
                LogError(ex, "SendDeathLink");
            }
        }
    }

    // ─── DeathLink hook ──────────────────────────────────────────────────────
    [HarmonyPatch(typeof(PlayerStatus), "Kill")]
    public class PlayerStatus_Kill_Patch
    {
        static void Postfix()
        {
            try
            {
                if (ArchipelagoPlugin.Instance != null)
                    ArchipelagoPlugin.Instance.SendDeathLink();
            }
            catch (Exception ex)
            {
                ArchipelagoPlugin.LogError(ex, "PlayerStatus.Kill Postfix");
            }
        }
    }

    // ─── Level completion hook ────────────────────────────────────────────────
    [HarmonyPatch(typeof(GameManager), "LevelDone")]
    public class GameManager_LevelDone_Patch
    {
        static void Postfix()
        {
            try
            {
                if (ArchipelagoPlugin.Instance == null) return;
                int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
                long locationId = 3310000 + sceneIndex;
                ArchipelagoPlugin.Instance.SendLocationCheck(locationId, $"Level {sceneIndex - 1} Completed");
            }
            catch (Exception ex)
            {
                ArchipelagoPlugin.LogError(ex, "GameManager.LevelDone Postfix");
            }
        }
    }

    // ─── Powerup collection logic ─────────────────────────────────────────────

    [HarmonyPatch]
    public class Powerup_Collect_Patches
    {
        static System.Collections.Generic.IEnumerable<System.Reflection.MethodBase> TargetMethods()
        {
            var methods = new System.Collections.Generic.List<System.Reflection.MethodBase>();
            methods.Add(AccessTools.Method(typeof(SwordPowerup), "Activate"));
            methods.Add(AccessTools.Method(typeof(DoubleJump), "Activate"));
            methods.Add(AccessTools.Method(typeof(RewindPickup), "Activate"));
            return methods.Where(m => m != null);
        }

        [HarmonyPrefix]
        static bool Prefix(object __instance) // Use object to be safe with different classes
        {
            try
            {
                if (ArchipelagoPlugin.Instance == null) return true;
                bool hasItem = false;
                long locId = 0;
                string label = "";
                int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

                if (__instance is SwordPowerup) 
                { 
                    hasItem = ArchipelagoPlugin.HasSword; 
                    locId = 3310100 + sceneIndex; 
                    label = $"Level {sceneIndex - 1} - Sword"; 
                }
                else if (__instance is DoubleJump) 
                { 
                    hasItem = ArchipelagoPlugin.HasDoubleJump; 
                    locId = 3310200 + sceneIndex; 
                    label = $"Level {sceneIndex - 1} - Double Jump"; 
                }
                else if (__instance is RewindPickup) 
                { 
                    hasItem = ArchipelagoPlugin.HasRewind; 
                    locId = 3310300 + sceneIndex; 
                    label = $"Level {sceneIndex - 1} - Rewind"; 
                }

                // Always send the location check
                if (locId != 0)
                    ArchipelagoPlugin.Instance.SendLocationCheck(locId, label);

                if (!hasItem)
                {
                    ArchipelagoPlugin.Instance.Logger.LogInfo($"{label} collected but NOT unlocked in Archipelago.");
                    ArchipelagoPlugin.Instance.AddNotification($"{label} (Locked)");
                    
                    // Still "consume" the pickup visually
                    if (__instance is MonoBehaviour mb)
                    {
                        mb.gameObject.SetActive(false);
                    }
                    
                    // Skip the actual effect
                    return false;
                }

                return true; // Allow the powerup to be granted
            }
            catch (Exception ex)
            {
                ArchipelagoPlugin.LogError(ex, "Powerup collection logic");
                return true;
            }
        }
    }

    // ─── Level locking hook (Redirection instead of Cancellation) ───────────
    [HarmonyPatch(typeof(UnityEngine.SceneManagement.SceneManager))]
    public class SceneManager_Lock_Patches
    {
        private static bool _isRedirecting = false;

        static System.Collections.Generic.IEnumerable<System.Reflection.MethodBase> TargetMethods()
        {
            var methods = new System.Collections.Generic.List<System.Reflection.MethodBase>();
            // Target the int-based LoadScene and LoadSceneAsync overloads
            methods.Add(AccessTools.Method(typeof(UnityEngine.SceneManagement.SceneManager), "LoadScene", new Type[] { typeof(int) }));
            methods.Add(AccessTools.Method(typeof(UnityEngine.SceneManagement.SceneManager), "LoadScene", new Type[] { typeof(int), typeof(UnityEngine.SceneManagement.LoadSceneMode) }));
            methods.Add(AccessTools.Method(typeof(UnityEngine.SceneManagement.SceneManager), "LoadSceneAsync", new Type[] { typeof(int) }));
            methods.Add(AccessTools.Method(typeof(UnityEngine.SceneManagement.SceneManager), "LoadSceneAsync", new Type[] { typeof(int), typeof(UnityEngine.SceneManagement.LoadSceneMode) }));
            return methods.Where(m => m != null);
        }

        [HarmonyPrefix]
        static bool Prefix(ref int __0) // Use positional argument __0 for the first parameter (scene index)
        {
            if (_isRedirecting) return true;

            try
            {
                if (ArchipelagoPlugin.Instance == null) return true;
                if (ArchipelagoPlugin.UnlockedLevels == null) return true;

                int requestedIndex = __0;

                // Check bounds
                if (requestedIndex >= 1 && requestedIndex < ArchipelagoPlugin.UnlockedLevels.Length)
                {
                    if (!ArchipelagoPlugin.UnlockedLevels[requestedIndex])
                    {
                        ArchipelagoPlugin.Instance.Logger.LogInfo($"Level {requestedIndex - 1} is locked. Redirecting to Menu (Index 0).");
                        ArchipelagoPlugin.Instance.AddNotification($"Level {requestedIndex - 1} is LOCKED");
                        
                        _isRedirecting = true;
                        __0 = 0; // Modify the first argument to 0 (Menu)
                        _isRedirecting = false;
                    }
                }
                return true; // Always allow the original method to run with the (possibly modified) index
            }
            catch (Exception ex)
            {
                _isRedirecting = false;
                ArchipelagoPlugin.LogError(ex, "SceneManager redirection logic");
                return true;
            }
        }
    }

    // ─── Enemy checks logic ──────────────────────────────────────────────────

    [HarmonyPatch]
    public class Enemy_Death_Patches
    {
        private static System.Collections.Generic.Dictionary<(int, string), (long, string)> enemyLocations = new System.Collections.Generic.Dictionary<(int, string), (long, string)> {
            { (0, "Enemy"), (3311000L, "Level 0 - The Poor Swordsman") },
            { (1, "EnemyArcherBlue"), (3311001L, "Level 1 - Blue Double Jump Archer") },
            { (2, "Enemy"), (3311002L, "Level 2 - The red swordman right at spawn") },
            { (2, "BlueEnemy"), (3311003L, "Level 2 - The blue Double Jump Swordsman") },
            { (3, "EnemyArcherBlue"), (3311004L, "Level 3 - The Blue Archer Across The Way") },
            { (3, "Enemy"), (3311005L, "Level 3 - The Red Swordman before barricade") },
            { (3, "Enemy (1)"), (3311006L, "Level 3 - The Red Swordman Right at the end") },
            { (4, "Enemy"), (3311007L, "Level 4 - The Red Challanger 1") },
            { (4, "Enemy (1)"), (3311008L, "Level 4 - The Red Challanger 2") },
            { (4, "BlueEnemy"), (3311009L, "Level 4 - The Blue Challanger") },
            { (4, "EnemyArcherBlue"), (3311010L, "Level 4 - The Blue Archer On Top") },
            { (4, "EnemyArcher"), (3311011L, "Level 4 - The Red Archer On Top") },
            { (5, "Enemy"), (3311012L, "Level 5 - The Red Enemy That Is Aproaching") },
            { (5, "EnemyArcher"), (3311013L, "Level 5 - The Annoying Red Archer On The Back") },
            { (5, "Enemy (2)"), (3311014L, "Level 5 - The Watcher Swordman 1") },
            { (5, "Enemy (1)"), (3311015L, "Level 5 - The Watcher Swordman 2") },
            { (5, "EnemyArcherBlue"), (3311016L, "Level 5 - The Blue King Archer") },
            { (6, "BlueEnemy"), (3311017L, "Level 6 - The Giant Blue Swordman At Start") },
            { (6, "EnemyArcher (2)"), (3311018L, "Level 6 - The Suprise Archer 1") },
            { (6, "EnemyArcher (3)"), (3311019L, "Level 6 - The Suprise Archer 2") },
            { (6, "EnemyArcher (1)"), (3311020L, "Level 6 - The Suprise Archer 3") },
            { (6, "EnemyArcher"), (3311021L, "Level 6 - The Suprise Archer 4") },
            { (7, "EnemyArcher"), (3311022L, "Level 7 - The Red Archer in front") },
            { (7, "Enemy"), (3311023L, "Level 7 - The Red Swordman on Top") },
            { (7, "Enemy (1)"), (3311024L, "Level 7 - The Red Swordman in the Box 1") },
            { (7, "Enemy (2)"), (3311025L, "Level 7 - The Red Swordman in the Box 2") },
            { (7, "EnemyArcher (1)"), (3311026L, "Level 7 - The Red Archer at the End") },
            { (7, "Enemy (3)"), (3311027L, "Level 7 - The Red Swordman at the End") },
            { (8, "Enemy"), (3311028L, "Level 8 - The Red Swordman after sliding") },
            { (8, "EnemyArcherBlue"), (3311029L, "Level 8 - The Blue Archer after that") },
            { (8, "Enemy (1)"), (3311030L, "Level 8 - The Red Swordman after Double Jump") },
            { (8, "Enemy (2)"), (3311031L, "Level 8 - The Red Swordman after The Red Swordman after Double Jump") },
            { (8, "Enemy (3)"), (3311032L, "Level 8 - The Red Swordman after The Red Swordman after The Red Swordman after Double Jump") },
            { (9, "Enemy"), (3311033L, "Level 9 - The Red Swordman at the start 1") },
            { (9, "Enemy (1)"), (3311034L, "Level 9 - The Red Swordman at the start 2") },
            { (9, "EnemyArcher"), (3311035L, "Level 9 - The Red Archer at the top") },
            { (9, "EnemyArcher (1)"), (3311036L, "Level 9 - The Red Archer at the elevator") },
            { (9, "BlueEnemy"), (3311037L, "Level 9 - The Blue Giant Swordman waiting for the elevator") },
            { (9, "EnemyArcher (2)"), (3311038L, "Level 9 - The Red Archer that annoys me so much that i want to end him right there right now") },
            { (10, "Enemy"), (3311039L, "Level 10 - The Red Swordman in the Room") },
            { (10, "EnemyArcher"), (3311040L, "Level 10 - The Red Archer at the Castle 1") },
            { (10, "EnemyArcher (1)"), (3311041L, "Level 10 - The Red Archer at the Castle 2") },
            { (10, "EnemyArcherBlue"), (3311042L, "Level 10 - The Blue Archer at the Castle") },
            { (10, "Enemy (1)"), (3311043L, "Level 10 - The Red Swordman at the end") },
            { (10, "EnemyArcher (3)"), (3311044L, "Level 10 - The Red Archer at the end") },
        };

        static System.Collections.Generic.IEnumerable<System.Reflection.MethodBase> TargetMethods()
        {
            var methods = new System.Collections.Generic.List<System.Reflection.MethodBase>();
            // Try to find RigidEnemy, IkEnemy, Archer, and Enemy
            // Note: Swordman was not found in the assembly, so we remove it.
            var types = new string[] { "RigidEnemy", "IkEnemy", "Archer", "Enemy" };
            var methodNames = new string[] { "KillRigidEnemy", "Kill", "Die" };

            foreach (var typeName in types)
            {
                var type = AccessTools.TypeByName(typeName);
                if (type != null)
                {
                    foreach (var methodName in methodNames)
                    {
                        var m = AccessTools.Method(type, methodName);
                        if (m != null) 
                        {
                            methods.Add(m);
                            // ArchipelagoPlugin.Instance.Logger.LogInfo($"Targeting {typeName}.{methodName} for enemy checks.");
                        }
                    }
                }
            }
            return methods;
        }

        [HarmonyPostfix]
        static void Postfix(object __instance)
        {
            try
            {
                if (ArchipelagoPlugin.Instance == null) return;
                
                // We need the gameObject. In Unity, scripts inherit from Component or MonoBehaviour
                GameObject go = null;
                if (__instance is MonoBehaviour mb) go = mb.gameObject;
                else if (__instance is Component comp) go = comp.gameObject;
                
                if (go == null) return;

                int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
                int levelNum = sceneIndex - 1;
                string objName = go.name;

                if (enemyLocations.TryGetValue((levelNum, objName), out var data))
                {
                    ArchipelagoPlugin.Instance.Logger.LogInfo($"Enemy Check: {data.Item2} ({objName}) in Level {levelNum}");
                    ArchipelagoPlugin.Instance.SendLocationCheck(data.Item1, data.Item2);
                }
            }
            catch (Exception ex)
            {
                ArchipelagoPlugin.LogError(ex, "Enemy_Death_Patches Postfix");
            }
        }
    }
}
