using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace TeleportMod;

/// <summary>
/// Teleport mod - save and teleport to locations
/// </summary>
[BepInPlugin("com.example.teleportmod", "Teleport Mod", "1.0.0")]
public class TeleportModPlugin : BasePlugin
{
    internal static ManualLogSource Logger;
    internal static Dictionary<int, Vector3> SavedPositions = new();
    internal static ConfigEntry<KeyCode> SaveKey;
    internal static ConfigEntry<KeyCode> TeleportKey;

    public override void Load()
    {
        Logger = Log;

        SaveKey = Config.Bind("Keys", "SavePositionKey", KeyCode.F9, "Key to save current position");
        TeleportKey = Config.Bind("Keys", "TeleportKey", KeyCode.F10, "Key to teleport to saved position");

        AddComponent<TeleportController>();
        Log.LogInfo("Teleport Mod loaded! F9=Save, F10=Teleport");
    }
}

public class TeleportController : MonoBehaviour
{
    private int currentSlot = 0;

    void Update()
    {
        // Number keys to select slot (1-5)
        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                currentSlot = i;
                TeleportModPlugin.Logger.LogInfo($"Selected slot {i + 1}");
            }
        }

        // Save position
        if (Input.GetKeyDown(TeleportModPlugin.SaveKey.Value))
        {
            var player = Player.Instance;
            if (player != null)
            {
                TeleportModPlugin.SavedPositions[currentSlot] = player.transform.position;
                TeleportModPlugin.Logger.LogInfo($"Position saved to slot {currentSlot + 1}");
            }
        }

        // Teleport to saved position
        if (Input.GetKeyDown(TeleportModPlugin.TeleportKey.Value))
        {
            if (TeleportModPlugin.SavedPositions.TryGetValue(currentSlot, out var pos))
            {
                var player = Player.Instance;
                if (player?.charController != null)
                {
                    player.charController.enabled = false;
                    player.transform.position = pos;
                    player.charController.enabled = true;
                    player.UpdateGameLocation(0, true);
                    TeleportModPlugin.Logger.LogInfo($"Teleported to slot {currentSlot + 1}");
                }
            }
            else
            {
                TeleportModPlugin.Logger.LogWarning($"No position saved in slot {currentSlot + 1}");
            }
        }
    }
}
