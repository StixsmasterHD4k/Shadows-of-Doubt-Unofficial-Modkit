using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace ShadowsOfDoubtMod.Patches;

/// <summary>
/// Patches for GameplayController - manages game state, money, evidence, cases, etc.
/// GameplayController fields:
///   - money (int) - player's money
///   - lockPicks (int) - player's lockpick count  
///   - socialCredit (int) - player's social credit
///   - evidenceDictionary - all discovered evidence
///   - factList - all discovered facts
///   - acquiredPasscodes - passcodes the player knows
///   - acquiredNumbers - phone numbers the player knows
///   - enforcers - list of police/enforcers
///   - crimeScenes - locations marked as crime scenes
/// </summary>
[HarmonyPatch]
public static class GameplayPatches
{
    /// <summary>
    /// Called when GameplayController initializes
    /// </summary>
    [HarmonyPatch(typeof(GameplayController), "Start")]
    [HarmonyPostfix]
    public static void GameplayController_Start_Postfix(GameplayController __instance)
    {
        Plugin.Log.LogInfo("GameplayController initialized");
    }

    /// <summary>
    /// Hook into money changes
    /// </summary>
    [HarmonyPatch(typeof(GameplayController), "AddMoney")]
    [HarmonyPrefix]
    public static void GameplayController_AddMoney_Prefix(GameplayController __instance, ref int amount, bool displayMessage, string message)
    {
        Plugin.Log.LogInfo($"Adding money: {amount} (current: {__instance.money})");
        // Modify amount if desired
        // amount *= 2; // double all money gains
    }

    /// <summary>
    /// Hook into passcode discovery
    /// </summary>
    [HarmonyPatch(typeof(GameplayController), "AddOrMergePasscodeData")]
    [HarmonyPostfix]
    public static void GameplayController_AddPasscode_Postfix(GameplayController __instance, GameplayController.Passcode newCode, bool displayMessage)
    {
        Plugin.Log.LogInfo($"Discovered passcode for: {newCode.name}");
    }
}

/// <summary>
/// Patches for SessionData - manages game time, weather, pause state, etc.
/// SessionData fields:
///   - play (bool) - whether game is unpaused
///   - gameTime (float) - current in-game time
///   - day (int) - current day
///   - isRaining (bool)
///   - currentWeather - weather state
///   - pauseState - current pause state
/// </summary>
[HarmonyPatch]
public static class SessionDataPatches
{
    [HarmonyPatch(typeof(SessionData), "PauseGame")]
    [HarmonyPostfix]
    public static void SessionData_PauseGame_Postfix(SessionData __instance, bool showPauseText, bool openDesktopMode, bool displayCursor)
    {
        Plugin.Log.LogInfo("Game paused");
    }

    [HarmonyPatch(typeof(SessionData), "ResumeGame")]
    [HarmonyPostfix]
    public static void SessionData_ResumeGame_Postfix(SessionData __instance)
    {
        Plugin.Log.LogInfo("Game resumed");
    }
}
