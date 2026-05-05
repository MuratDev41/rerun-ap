using System;
using System.Linq;
using BepInEx;
using HarmonyLib;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using UnityEngine;

namespace RerunArchipelago
{
    [BepInPlugin("com.archipelago.rerun", "RE:RUN Archipelago", "1.0.0")]
    public class ArchipelagoPlugin : BaseUnityPlugin
    {
        public static ArchipelagoPlugin Instance;
        public ArchipelagoSession Session;
        public new BepInEx.Logging.ManualLogSource Logger;

        // Settings for Archipelago connection
        private string serverUrl = "archipelago.gg:38281";
        private string slotName = "Player1";
        private string password = "";

        // UI State
        private bool showMenu = true;
        private Rect windowRect = new Rect(20, 20, 320, 220);

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

            // Initialize unlocked levels (Menu only)
            UnlockedLevels[0] = true; // Menu

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

                Logger.LogInfo("Successfully connected to Archipelago!");
                AddNotification("Connected to Archipelago!");
                showMenu = false;

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

            GUILayout.Label("Server URL:");
            serverUrl = GUILayout.TextField(serverUrl);

            GUILayout.Label("Slot Name:");
            slotName = GUILayout.TextField(slotName);

            GUILayout.Label("Password:");
            password = GUILayout.TextField(password);

            GUILayout.Space(10);

            if (GUILayout.Button("Connect"))
                Connect();

            string status = Session != null ? "● Connected" : "● Disconnected";
            if (_goalReached) status = "★ GOAL REACHED ★";
            GUILayout.Label(status);

            GUILayout.Space(6);
            GUILayout.Label($"Sword: {(HasSword ? "✓" : "✗")}  DoubleJump: {(HasDoubleJump ? "✓" : "✗")}  Rewind: {(HasRewind ? "✓" : "✗")}");
            
            string unlockedStr = "Levels: ";
            for (int i = 0; i <= 10; i++)
            {
                if (UnlockedLevels[i + 1]) unlockedStr += $"{i} ";
            }
            GUILayout.Label(unlockedStr);

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
            { (0, "Enemy"), (3311000L, "The Poor Swordsman") },
            { (1, "EnemyArcherBlue"), (3311001L, "Blue Double Jump Archer") },
            { (2, "Enemy"), (3311002L, "The red swordman right at spawn") },
            { (2, "BlueEnemy"), (3311003L, "The blue Double Jump Swordsman") },
            { (3, "EnemyArcherBlue"), (3311004L, "The Blue Archer Across The Way") },
            { (3, "Enemy"), (3311005L, "The Red Swordman before barricade") },
            { (3, "Enemy (1)"), (3311006L, "The Red Swordman Right at the end") },
            { (4, "Enemy"), (3311007L, "The Red Challanger 1") },
            { (4, "Enemy (1)"), (3311008L, "The Red Challanger 2") },
            { (4, "BlueEnemy"), (3311009L, "The Blue Challanger") },
            { (4, "EnemyArcherBlue"), (3311010L, "The Blue Archer On Top") },
            { (4, "EnemyArcher"), (3311011L, "The Red Archer On Top") },
            { (5, "Enemy"), (3311012L, "The Red Enemy That Is Aproaching") },
            { (5, "EnemyArcher"), (3311013L, "The Annoying Red Archer On The Back") },
            { (5, "Enemy (2)"), (3311014L, "The Watcher Swordman 1") },
            { (5, "Enemy (1)"), (3311015L, "The Watcher Swordman 2") },
            { (5, "EnemyArcherBlue"), (3311016L, "The Blue King Archer") },
            { (6, "BlueEnemy"), (3311017L, "The Giant Blue Swordman At Start") },
            { (6, "EnemyArcher (2)"), (3311018L, "The Suprise Archer 1") },
            { (6, "EnemyArcher (3)"), (3311019L, "The Suprise Archer 2") },
            { (6, "EnemyArcher (1)"), (3311020L, "The Suprise Archer 3") },
            { (6, "EnemyArcher"), (3311021L, "The Suprise Archer 4") },
            { (7, "EnemyArcher"), (3311022L, "The Red Archer in front") },
            { (7, "Enemy"), (3311023L, "The Red Archer on Top") },
            { (7, "Enemy (1)"), (3311024L, "The Red Swordman in the Box 1") },
            { (7, "Enemy (2)"), (3311025L, "The Red Swordman in the Box 2") },
            { (7, "EnemyArcher (1)"), (3311026L, "The Red Archer at the End") },
            { (7, "Enemy (3)"), (3311027L, "The Red Swordman at the End") },
            { (8, "Enemy"), (3311028L, "The Red Swordman after sliding") },
            { (8, "EnemyArcherBlue"), (3311029L, "The Blue Archer after that") },
            { (8, "Enemy (1)"), (3311030L, "The Red Swordman after Double Jump") },
            { (8, "Enemy (2)"), (3311031L, "The Red Swordman after The Red Swordman after Double Jump") },
            { (8, "Enemy (3)"), (3311032L, "The Red Swordman after The Red Swordman after The Red Swordman after Double Jump") },
            { (9, "Enemy"), (3311033L, "The Red Swordman at the start 1") },
            { (9, "Enemy (1)"), (3311034L, "The Red Swordman at the start 2") },
            { (9, "EnemyArcher"), (3311035L, "The Red Archer at the top") },
            { (9, "EnemyArcher (1)"), (3311036L, "The Red Archer at the elevator") },
            { (9, "BlueEnemy"), (3311037L, "The Blue Giant Swordman waiting for the elevator") },
            { (9, "EnemyArcher (2)"), (3311038L, "The Red Archer that annoys me so much that i want to end him right there right now") },
            { (10, "Enemy"), (3311039L, "The Red Swordman in the Room") },
            { (10, "EnemyArcher"), (3311040L, "The Red Archer at the Castle 1") },
            { (10, "EnemyArcher (1)"), (3311041L, "The Red Archer at the Castle 2") },
            { (10, "EnemyArcherBlue"), (3311042L, "The Blue Archer at the Castle") },
            { (10, "Enemy (1)"), (3311043L, "The Red Swordman at the end") },
            { (10, "EnemyArcher (3)"), (3311044L, "The Red Archer at the end") },
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
