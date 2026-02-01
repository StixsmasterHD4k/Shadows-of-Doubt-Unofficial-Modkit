using HarmonyLib;
using UnityEngine;

namespace ShadowsOfDoubtMod.Patches;

/// <summary>
/// Patches for Citizen and Human classes
/// Human fields:
///   - humanID (int)
///   - citizenName, firstName, surName (string)
///   - gender (Human.Gender)
///   - birthday (string)
///   - home (NewAddress)
///   - job (Occupation)
///   - partner (Citizen)
///   - bloodType (Human.BloodType)
///   - characterTraits - list of personality traits
///   - nourishment, energy, hygiene, stress (float) - status stats
///   - isAsleep, isUnconscious, isDead (bool)
///   - currentRoom (NewRoom)
///   - currentGameLocation (NewGameLocation)
/// </summary>
[HarmonyPatch]
public static class CitizenPatches
{
    /// <summary>
    /// Called when a citizen wakes up
    /// </summary>
    [HarmonyPatch(typeof(Human), "WakeUp")]
    [HarmonyPostfix]
    public static void Human_WakeUp_Postfix(Human __instance)
    {
        // Don't log for player
        if (__instance is Player) return;
        // Plugin.Log.LogInfo($"{__instance.citizenName} woke up");
    }

    /// <summary>
    /// Hook into citizen conversations
    /// </summary>
    [HarmonyPatch(typeof(Human), "StartConversation")]
    [HarmonyPostfix]
    public static void Human_StartConversation_Postfix(Human __instance, Human other)
    {
        if (__instance is Player && other != null)
        {
            Plugin.Log.LogInfo($"Player started conversation with: {other.citizenName}");
        }
    }
}

/// <summary>
/// Patches for NewAIController - handles citizen AI behavior
/// NewAIController fields:
///   - human (Human) - the controlled human
///   - currentGoal (NewAIGoal)
///   - currentAction (NewAIAction)
///   - alertness (float)
///   - investigating (bool)
///   - spotted - list of spotted actors
/// </summary>
[HarmonyPatch]
public static class AIPatches
{
    [HarmonyPatch(typeof(NewAIController), "SetInvestigation")]
    [HarmonyPostfix]
    public static void AI_SetInvestigation_Postfix(NewAIController __instance, bool val)
    {
        if (val && __instance.human != null)
        {
            // Plugin.Log.LogInfo($"{__instance.human.citizenName} started investigating");
        }
    }
}
