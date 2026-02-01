using UnityEngine;
using BepInEx.Configuration;

namespace ShadowsOfDoubtMod.Utilities;

/// <summary>
/// Debug/cheat commands - press configured keys to trigger
/// </summary>
public static class DebugCommands
{
    public static bool GodModeEnabled { get; set; } = false;
    public static bool InfiniteMoney { get; set; } = false;
    public static bool NoClipEnabled { get; set; } = false;

    /// <summary>Call this from ModController.Update()</summary>
    public static void ProcessInput()
    {
        // F1 - Toggle God Mode
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GodModeEnabled = !GodModeEnabled;
            Plugin.Log.LogInfo($"God Mode: {(GodModeEnabled ? "ON" : "OFF")}");
        }

        // F2 - Add Money
        if (Input.GetKeyDown(KeyCode.F2))
        {
            GameHelpers.AddMoney(1000, "Debug money added");
            Plugin.Log.LogInfo("Added 1000 money");
        }

        // F3 - Full Heal
        if (Input.GetKeyDown(KeyCode.F3))
        {
            var player = GameHelpers.GetPlayer();
            if (player != null)
            {
                player.health = player.maxHealth;
                player.nourishment = 1f;
                player.energy = 1f;
                player.hygiene = 1f;
                Plugin.Log.LogInfo("Player fully healed");
            }
        }

        // F4 - Reveal Murder Info (spoiler!)
        if (Input.GetKeyDown(KeyCode.F4))
        {
            var murder = GameHelpers.GetActiveMurder();
            if (murder != null)
            {
                Plugin.Log.LogInfo($"=== MURDER INFO ===");
                Plugin.Log.LogInfo($"Victim: {murder.victim?.citizenName ?? "Unknown"}");
                Plugin.Log.LogInfo($"Murderer: {murder.murderer?.citizenName ?? "Unknown"}");
                Plugin.Log.LogInfo($"State: {murder.state}");
            }
        }

        // F6 - Teleport to murder victim location
        if (Input.GetKeyDown(KeyCode.F6))
        {
            var victim = GameHelpers.GetMurderVictim();
            if (victim?.transform != null)
            {
                GameHelpers.TeleportPlayer(victim.transform.position + Vector3.up);
                Plugin.Log.LogInfo($"Teleported to victim: {victim.citizenName}");
            }
        }

        // F7 - Skip time by 1 hour
        if (Input.GetKeyDown(KeyCode.F7))
        {
            var session = GameHelpers.GetSessionData();
            if (session != null)
            {
                session.gameTime += 1f;
                if (session.gameTime >= 24f) 
                {
                    session.gameTime -= 24f;
                    session.day++;
                }
                Plugin.Log.LogInfo($"Time: {session.gameTime:F1}:00, Day {session.day}");
            }
        }

        // F8 - Add lockpicks
        if (Input.GetKeyDown(KeyCode.F8))
        {
            var gc = GameHelpers.GetGameplayController();
            if (gc != null)
            {
                gc.lockPicks += 10;
                Plugin.Log.LogInfo($"Lockpicks: {gc.lockPicks}");
            }
        }

        // F9 - List nearby citizens
        if (Input.GetKeyDown(KeyCode.F9))
        {
            var player = GameHelpers.GetPlayer();
            if (player?.currentRoom != null)
            {
                Plugin.Log.LogInfo($"=== Citizens in current room ===");
                // Access room's citizen list through nodes
            }
        }
    }
}
