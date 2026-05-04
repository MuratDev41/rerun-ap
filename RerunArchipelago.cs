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

        private void Awake()
        {
            Instance = this;
            Logger = base.Logger;
            Logger.LogInfo("Plugin RE:RUN Archipelago is loaded!");

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
            Logger.LogInfo($"Received item: {item.ItemName}");
            AddNotification($"Received: {item.ItemName}");

            switch (item.ItemName)
            {
                case "Sword":
                    if (!HasSword)
                    {
                        HasSword = true;
                        // Re-enable any existing SwordPowerup instances in the scene
                        foreach (var pickup in FindObjectsOfType<SwordPowerup>())
                            pickup.gameObject.SetActive(true);
                    }
                    break;
                case "Double Jump":
                    if (!HasDoubleJump)
                    {
                        HasDoubleJump = true;
                        foreach (var pickup in FindObjectsOfType<DoubleJump>())
                            pickup.gameObject.SetActive(true);
                    }
                    break;
                case "Rewind":
                    if (!HasRewind)
                    {
                        HasRewind = true;
                        foreach (var pickup in FindObjectsOfType<RewindPickup>())
                            pickup.gameObject.SetActive(true);
                    }
                    break;
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
            // Toggle menu with F3
            if (Input.GetKeyDown(KeyCode.F3))
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
                windowRect = GUI.Window(0, windowRect, DrawMenu, "Archipelago Connection  [F3]");
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
            GUILayout.Label(status);

            GUILayout.Space(6);
            GUILayout.Label($"Sword: {(HasSword ? "✓" : "✗")}  DoubleJump: {(HasDoubleJump ? "✓" : "✗")}  Rewind: {(HasRewind ? "✓" : "✗")}");

            GUI.DragWindow();
        }

        public void SendLocationCheck(long locationId, string label)
        {
            if (Session == null) return;
            if (!Session.Locations.AllLocationsChecked.Contains(locationId))
            {
                Logger.LogInfo($"Sending check: {label} (ID: {locationId})");
                AddNotification($"Sent Check: {label}");
                Session.Locations.CompleteLocationChecks(locationId);
            }
        }
    }

    // ─── Level completion hook ────────────────────────────────────────────────
    [HarmonyPatch(typeof(GameManager), "LevelDone")]
    public class GameManager_LevelDone_Patch
    {
        static void Postfix()
        {
            int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            long locationId = 3310000 + sceneIndex;
            ArchipelagoPlugin.Instance.SendLocationCheck(locationId, $"Level {sceneIndex}");
        }
    }

    // ─── Powerup pickup patches ───────────────────────────────────────────────

    [HarmonyPatch(typeof(SwordPowerup), "Start")]
    public class SwordPowerup_Start_Patch
    {
        static void Postfix(SwordPowerup __instance)
        {
            if (!ArchipelagoPlugin.HasSword)
            {
                __instance.gameObject.SetActive(false);
            }
        }
    }

    [HarmonyPatch(typeof(DoubleJump), "Start")]
    public class DoubleJump_Start_Patch
    {
        static void Postfix(DoubleJump __instance)
        {
            if (!ArchipelagoPlugin.HasDoubleJump)
            {
                __instance.gameObject.SetActive(false);
            }
        }
    }

    [HarmonyPatch(typeof(RewindPickup), "Start")]
    public class RewindPickup_Start_Patch
    {
        static void Postfix(RewindPickup __instance)
        {
            if (!ArchipelagoPlugin.HasRewind)
            {
                __instance.gameObject.SetActive(false);
            }
        }
    }

    // ─── Send location checks when pickups are actually collected ─────────────

    [HarmonyPatch(typeof(SwordPowerup), "OnTriggerEnter")]
    public class SwordPowerup_Collect_Patch
    {
        static void Postfix()
        {
            ArchipelagoPlugin.Instance.SendLocationCheck(3310401, "Sword Picked Up");
        }
    }

    [HarmonyPatch(typeof(DoubleJump), "OnTriggerEnter")]
    public class DoubleJump_Collect_Patch
    {
        static void Postfix()
        {
            ArchipelagoPlugin.Instance.SendLocationCheck(3310402, "Double Jump Picked Up");
        }
    }

    [HarmonyPatch(typeof(RewindPickup), "OnTriggerEnter")]
    public class RewindPickup_Collect_Patch
    {
        static void Postfix()
        {
            ArchipelagoPlugin.Instance.SendLocationCheck(3310403, "Rewind Picked Up");
        }
    }
}
