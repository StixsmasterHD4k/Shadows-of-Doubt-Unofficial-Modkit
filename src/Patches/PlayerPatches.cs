using HarmonyLib;
using UnityEngine;

namespace ShadowsOfDoubtMod.Patches;

/// <summary>
/// Harmony patches for the Player class
/// Player inherits from Human, which inherits from Actor -> Controller -> MonoBehaviour
/// </summary>
[HarmonyPatch]
public static class PlayerPatches
{
    /// <summary>
    /// Called when Player.Update runs
    /// Player fields: fpsMode, fps, charController, cam, money (via GameplayController), health, etc.
    /// </summary>
    [HarmonyPatch(typeof(Player), "Update")]
    [HarmonyPostfix]
    public static void Player_Update_Postfix(Player __instance)
    {
        // Access player state
        // __instance.fpsMode - bool, whether in first person mode
        // __instance.charController - CharacterController
        // __instance.currentGameLocation - NewGameLocation
        // __instance.currentRoom - NewRoom
        // __instance.transitionActive - bool
    }

    /// <summary>
    /// Called when player's game location changes
    /// </summary>
    [HarmonyPatch(typeof(Player), "OnGameLocationChange")]
    [HarmonyPostfix]
    public static void Player_OnGameLocationChange_Postfix(Player __instance, bool enableSocialSightings, bool forceDisableLocationMemory)
    {
        if (__instance.currentGameLocation != null)
        {
            Plugin.Log.LogInfo($"Player entered: {__instance.currentGameLocation.name}");
        }
    }

    /// <summary>
    /// Called when player takes damage
    /// </summary>
    [HarmonyPatch(typeof(Human), "RecieveDamage")]
    [HarmonyPrefix]
    public static bool Human_RecieveDamage_Prefix(Human __instance, float amount, Actor attacker, ref bool __result)
    {
        // Check if this is the player
        if (__instance is Player player)
        {
            Plugin.Log.LogInfo($"Player taking {amount} damage");
            // Return false to prevent original method (god mode)
            // Return true to allow normal damage
        }
        return true;
    }

    /// <summary>
    /// Called when player dies
    /// </summary>
    [HarmonyPatch(typeof(Human), "SetDeath")]
    [HarmonyPrefix]
    public static void Human_SetDeath_Prefix(Human __instance, Human.Death.CauseOfDeath cause, bool murder, Human killer)
    {
        if (__instance is Player)
        {
            Plugin.Log.LogInfo($"Player died! Cause: {cause}");
        }
    }
}
