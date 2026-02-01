using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace ShadowsOfDoubtMod.Patches;

/// <summary>
/// Patches for Case class - manages detective cases and objectives
/// Case fields:
///   - caseID (int)
///   - name (string)
///   - caseType - type of case (murder, missing person, etc.)
///   - isActive (bool)
///   - isSolved (bool)
///   - elements - list of Case.CaseElement (evidence on the case board)
///   - resolves - list of Case.ResolveQuestion (objectives)
///   - reward (int) - base reward
///   - deadline - time limit
/// </summary>
[HarmonyPatch]
public static class CasePatches
{
    [HarmonyPatch(typeof(Case), "OnCaseSolved")]
    [HarmonyPostfix]
    public static void Case_OnCaseSolved_Postfix(Case __instance)
    {
        Plugin.Log.LogInfo($"Case solved: {__instance.name}");
    }

    [HarmonyPatch(typeof(Case), "SetActive")]
    [HarmonyPostfix]
    public static void Case_SetActive_Postfix(Case __instance, bool val)
    {
        if (val)
        {
            Plugin.Log.LogInfo($"Case activated: {__instance.name}");
        }
    }
}

/// <summary>
/// Patches for MurderController - handles murder generation and state
/// MurderController.Murder fields:
///   - murderer (Human) - the killer
///   - victim (Human) - the victim
///   - preset (MurderPreset) - murder configuration
///   - mo (MurderMO) - modus operandi
///   - state (MurderState) - current murder state
///   - murderWeapon (Interactable)
///   - killLocation - where the murder happens
/// </summary>
[HarmonyPatch]
public static class MurderPatches
{
    [HarmonyPatch(typeof(MurderController), "SetMurderState")]
    [HarmonyPostfix]
    public static void MurderController_SetState_Postfix(MurderController __instance, MurderController.Murder murder, MurderController.MurderState newState)
    {
        if (murder?.victim != null)
        {
            Plugin.Log.LogInfo($"Murder state changed: {murder.victim.citizenName} - State: {newState}");
        }
    }
}

/// <summary>
/// Patches for Evidence class - handles all evidence types
/// Evidence fields:
///   - evID (string) - unique evidence ID
///   - evName (string) - display name
///   - discovered (bool)
///   - preset (EvidencePreset)
///   - factLinks - connected facts
///   - dataKeys - data key matches
/// </summary>
[HarmonyPatch]
public static class EvidencePatches
{
    [HarmonyPatch(typeof(Evidence), "Discover")]
    [HarmonyPostfix]
    public static void Evidence_Discover_Postfix(Evidence __instance)
    {
        Plugin.Log.LogInfo($"Evidence discovered: {__instance.evName}");
    }
}
