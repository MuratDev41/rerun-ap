using System;
using System.Linq;
using BepInEx;
using HarmonyLib;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
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
        private float _lastDeathTime = 0f;

        // Settings for Archipelago connection
        private string serverUrl = "archipelago.gg:38281";
        private string slotName = "Player1";
        private string password = "";

        // UI State
        private bool showMenu = true;
        private Rect windowRect = new Rect(20, 20, 320, 320);

        // Chat Log (Bottom-Left)
        public struct ChatMessage
        {
            public string Text;
            public float Timer;
        }
        private System.Collections.Generic.List<ChatMessage> chatLog = new System.Collections.Generic.List<ChatMessage>();

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

            // Initialize unlocked levels (Menu always accessible)
            UnlockedLevels[0] = true; // Menu

            var harmony = new Harmony("com.archipelago.rerun.patches");

            // Patch non-death-link patches all at once (these are known-safe)
            harmony.PatchAll();

            // Manually patch DeathLink hooks with fallback so missing methods don't crash the plugin
            var postfix = new HarmonyMethod(typeof(DeathLinkHooks), nameof(DeathLinkHooks.SendDeathLinkPostfix));
            foreach (var (typeName, methodName) in new[]
            {
                ("PlayerStatus",   "Kill"),
                ("PlayerMovement", "Kill"),
                ("PlayerMovement", "Die"),
            })
            {
                try
                {
                    var type   = AccessTools.TypeByName(typeName);
                    var method = type != null ? AccessTools.DeclaredMethod(type, methodName) : null;
                    if (method != null)
                    {
                        harmony.Patch(method, postfix: postfix);
                        Logger.LogInfo($"[DeathLink] Patched {typeName}.{methodName}");
                    }
                    else
                    {
                        Logger.LogWarning($"[DeathLink] Skipped {typeName}.{methodName} (not found)");
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogWarning($"[DeathLink] Could not patch {typeName}.{methodName}: {ex.Message}");
                }
            }
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
                    AddLog($"<color=red>Connection failed: {err}</color>");
                    return;
                }

                var loginSuccess = (LoginSuccessful)result;
                var slotData = loginSuccess.SlotData;

                Logger.LogInfo("Successfully connected to Archipelago!");
                Logger.LogInfo("--- Slot Data Received ---");
                foreach (var kvp in slotData)
                {
                    Logger.LogInfo($"   {kvp.Key}: {kvp.Value}");
                }
                Logger.LogInfo("--------------------------");
                AddLog("<color=green>Connected to Archipelago!</color>");
                showMenu = false;

                // Listen for server messages (chat/activity) using MessageLog for automatic ID resolution
                Session.MessageLog.OnMessageReceived += (message) => {
                    string fullText = "";
                    foreach (var part in message.Parts)
                    {
                        string text = part.Text;
                        string color = "white";

                        if (part.Type == Archipelago.MultiClient.Net.MessageLog.Parts.MessagePartType.Player)
                        {
                            bool isMe = false;
                            if (text == slotName || text == Session.Players.GetPlayerAlias(Session.ConnectionInfo.Slot))
                                isMe = true;
                            
                            color = isMe ? "#ee00ee" : "#f4f4ce"; // Custom player colors
                        }
                        else if (part.Type == Archipelago.MultiClient.Net.MessageLog.Parts.MessagePartType.Item)
                        {
                            var itemPart = part as Archipelago.MultiClient.Net.MessageLog.Parts.ItemMessagePart;
                            if (itemPart != null)
                            {
                                if ((itemPart.Flags & Archipelago.MultiClient.Net.Enums.ItemFlags.Advancement) != 0)
                                    color = "#af99ef"; // Progressive
                                else if ((itemPart.Flags & Archipelago.MultiClient.Net.Enums.ItemFlags.Trap) != 0)
                                    color = "#fa7f72"; // Trap
                                else if ((itemPart.Flags & Archipelago.MultiClient.Net.Enums.ItemFlags.NeverExclude) != 0)
                                    color = "#6d8be8"; // Useful
                                else
                                    color = "#06eeee"; // Filler
                            }
                            else
                            {
                                color = "#06eeee"; // Filler fallback
                            }
                        }
                        else if (part.Type == Archipelago.MultiClient.Net.MessageLog.Parts.MessagePartType.Location)
                        {
                            color = "#00FF00"; // Greenish
                        }

                        fullText += $"<color={color}>{text}</color>";
                    }
                    AddLog(fullText);
                };

                // DeathLink Setup
                Logger.LogInfo("Initializing DeathLink...");
                if (slotData.TryGetValue("death_link", out var dl))
                {
                    string dlStr = dl.ToString().ToLower();
                    Logger.LogInfo($"DeathLink slot data value: {dlStr}");
                    if (dlStr == "true" || dlStr == "1")
                    {
                        deathLinkService = Session.CreateDeathLinkService();
                        deathLinkService.OnDeathLinkReceived += OnDeathLinkReceived;
                        deathLinkService.EnableDeathLink();
                        Logger.LogInfo("DeathLink service created and enabled.");
                    }
                    else
                    {
                        Logger.LogInfo("DeathLink is disabled in slot data.");
                    }
                }
                else
                {
                    Logger.LogInfo("DeathLink key not found in slot data.");
                }

                if (slotData.TryGetValue("death_link_amnesty", out var am))
                {
                    Logger.LogInfo($"DeathLink Amnesty raw value: {am}");
                    if (int.TryParse(am.ToString(), out deathLinkAmnesty))
                        Logger.LogInfo($"DeathLink Amnesty parsed as: {deathLinkAmnesty}");
                    else
                        Logger.LogWarning($"Failed to parse DeathLink Amnesty: {am}");
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
                AddLog($"Error: {ex.Message}");
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



        public void AddLog(string message)
        {
            chatLog.Add(new ChatMessage { Text = message, Timer = 4f });
            if (chatLog.Count > 6) chatLog.RemoveAt(0);
        }

        private float _tintTimer = 0f;

        private void Update()
        {
            // Toggle menu with P
            if (Input.GetKeyDown(KeyCode.P))
                showMenu = !showMenu;

            // Log timer
            for (int i = chatLog.Count - 1; i >= 0; i--)
            {
                var msg = chatLog[i];
                msg.Timer -= Time.deltaTime;
                if (msg.Timer <= 0)
                    chatLog.RemoveAt(i);
                else
                    chatLog[i] = msg; // Update back in list
            }

            if (showMenu)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            // Tint level select buttons
            if (Session != null && UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
            {
                _tintTimer -= Time.deltaTime;
                if (_tintTimer <= 0)
                {
                    _tintTimer = 1.0f; // Check every second

                    var buttons = UnityEngine.Object.FindObjectsOfType<UnityEngine.UI.Button>();
                    foreach (var btn in buttons)
                    {
                        int levelNum = -1;
                        
                        // Look for the level number in all text components (Standard and TMPro)
                        // This avoids accidentally picking up the timer text
                        var texts = btn.GetComponentsInChildren<UnityEngine.UI.Text>();
                        foreach (var t in texts)
                        {
                            if (t.text.ToUpper().Contains("LEVEL"))
                            {
                                string numStr = System.Text.RegularExpressions.Regex.Match(t.text, @"\d+").Value;
                                int.TryParse(numStr, out levelNum);
                                break;
                            }
                        }

                        if (levelNum == -1)
                        {
                            foreach (var comp in btn.GetComponentsInChildren<MonoBehaviour>())
                            {
                                if (comp.GetType().Name.Contains("TextMeshProUGUI"))
                                {
                                    var prop = Traverse.Create(comp).Property("text");
                                    if (prop.PropertyExists())
                                    {
                                        string tText = prop.GetValue<string>();
                                        if (tText != null && tText.ToUpper().Contains("LEVEL"))
                                        {
                                            string numStr = System.Text.RegularExpressions.Regex.Match(tText, @"\d+").Value;
                                            int.TryParse(numStr, out levelNum);
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (levelNum >= 0 && levelNum <= 10)
                        {
                            long locId = 3310000 + (levelNum + 1); // Level 0 is scene 1
                            bool unlocked = UnlockedLevels[levelNum + 1];
                            bool completed = Session.Locations.AllLocationsChecked.Contains(locId) || _sentLocations.Contains(locId);
                            
                            // Tint all images in the button hierarchy to ensure visibility
                            var images = btn.GetComponentsInChildren<UnityEngine.UI.Image>();
                            foreach (var img in images)
                            {
                                if (completed)
                                    img.color = new UnityEngine.Color(0.5f, 1f, 0.5f, 1f); // Green for completed
                                else if (!unlocked)
                                    img.color = new UnityEngine.Color(0.3f, 0.3f, 0.3f, 1f); // Dark gray for locked
                                else
                                    img.color = UnityEngine.Color.white; // Normal for playable
                            }
                        }
                    }
                }
            }
        }

        private void OnGUI()
        {
            // Draw Chat Logs (bottom-left)
            if (chatLog.Count > 0)
            {
                GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.LowerLeft;
                style.fontSize = 14; // 25% bigger than 11
                style.richText = true;
                style.normal.textColor = UnityEngine.Color.white;
                
                GUIStyle shadowStyle = new GUIStyle(style);
                shadowStyle.normal.textColor = UnityEngine.Color.black;

                float startY = Screen.height - 20;
                float lineH = 19;

                // Restore alpha after drawing logs
                UnityEngine.Color oldColor = GUI.color;

                for (int i = chatLog.Count - 1; i >= 0; i--)
                {
                    var msg = chatLog[i];
                    float alpha = Mathf.Clamp01(msg.Timer / 1.0f); // Fade out in the last second
                    if (msg.Timer > 1.0f) alpha = 1.0f;
                    
                    UnityEngine.Color c = GUI.color;
                    c.a = alpha;
                    GUI.color = c;

                    Rect logRect = new Rect(20, startY - (chatLog.Count - i) * lineH, 800, lineH);
                    
                    // Draw shadow (strip rich text tags so it renders completely black)
                    string cleanText = System.Text.RegularExpressions.Regex.Replace(msg.Text, "<.*?>", string.Empty);
                    GUI.Label(new Rect(logRect.x + 1, logRect.y + 1, logRect.width, logRect.height), cleanText, shadowStyle);
                    
                    // Draw main text
                    GUI.Label(logRect, msg.Text, style);
                }

                GUI.color = oldColor;
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
                GUILayout.Label($"Checks Found: {found} / 72");

                GUILayout.Space(10);
                if (GUILayout.Button("Disconnect"))
                {
                    try { Session.Socket.DisconnectAsync(); } catch { }
                    Session = null;
                    Logger.LogInfo("Disconnected from Archipelago.");
                    AddLog("<color=red>Disconnected from Archipelago.</color>");
                }
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
                AddLog("★ GOAL REACHED! ★");
            }
        }

        private void OnDeathLinkReceived(DeathLink deathLink)
        {
            try
            {
                Logger.LogInfo($"[DeathLink] Received from {deathLink.Source} at {deathLink.Timestamp}. Cause: {deathLink.Cause}");
                isReceivingDeath = true;
                
                // Find player and kill them
                var player = GameObject.FindObjectOfType<PlayerStatus>();
                if (player != null)
                {
                    AccessTools.Method(typeof(PlayerStatus), "Kill").Invoke(player, null);
                    AddLog($"<color=red>DeathLink from {deathLink.Source}: {deathLink.Cause}</color>");
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
            if (deathLinkService == null)
            {
                Logger.LogInfo("[DeathLink] Not sending: Service is null (DeathLink likely disabled).");
                return;
            }
            if (isReceivingDeath)
            {
                Logger.LogInfo("[DeathLink] Not sending: Currently processing a received DeathLink.");
                return;
            }

            // Prevent double-counting multiple calls for the same death (e.g. within 1 second)
            if (Time.time - _lastDeathTime < 1.0f) return;
            _lastDeathTime = Time.time;

            deathCounter++;
            // Send when counter hits amnesty (e.g. if amnesty is 5, send on 5th death)
            if (deathCounter < deathLinkAmnesty)
            {
                Logger.LogInfo($"[DeathLink] Amnesty: {deathCounter}/{deathLinkAmnesty}");
                AddLog($"DeathLink Amnesty: {deathCounter}/{deathLinkAmnesty}");
                return;
            }

            deathCounter = 0;
            try
            {
                Logger.LogInfo($"[DeathLink] Sending death link from {slotName} (Amnesty reached)...");
                var deathLink = new DeathLink(slotName, $"{slotName} had a skill issue.");
                deathLinkService.SendDeathLink(deathLink);
                Logger.LogInfo("[DeathLink] Successfully sent.");
                AddLog("<color=red>DeathLink Sent!</color>");
            }
            catch (Exception ex)
            {
                LogError(ex, "SendDeathLink");
            }
        }
    }

    // ─── DeathLink hooks (applied manually in Awake to gracefully skip missing methods) ───
    public static class DeathLinkHooks
    {
        public static void SendDeathLinkPostfix() { ArchipelagoPlugin.Instance?.SendDeathLink(); }
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
                    ArchipelagoPlugin.Instance.AddLog($"{label} (Locked)");
                    
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
                        ArchipelagoPlugin.Instance.AddLog($"Level {requestedIndex - 1} is LOCKED");
                        
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
