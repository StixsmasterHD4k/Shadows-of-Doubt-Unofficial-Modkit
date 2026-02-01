using HarmonyLib;
using UnityEngine;

namespace ShadowsOfDoubtMod.Patches;

/// <summary>
/// Patches for Interactable class - base for all interactive objects
/// Interactable fields:
///   - id (int) - unique world ID
///   - name (string)
///   - preset (InteractablePreset) - defines object type and behavior
///   - node (NewNode) - current node position
///   - evidence (Evidence) - associated evidence if any
///   - belongsTo (Human) - owner
///   - writer, reciever (Human) - for notes/messages
///   - locked (bool)
///   - val (float) - monetary value
///   - inInventory (Human) - who is carrying this
///   - furnitureParent (FurnitureLocation)
///   - spawnedObject (GameObject) - the actual Unity object
///   - passcode (GameplayController.Passcode) - associated passcode
/// </summary>
[HarmonyPatch]
public static class InteractablePatches
{
    /// <summary>
    /// Called when player interacts with an object
    /// </summary>
    [HarmonyPatch(typeof(Interactable), "OnInteraction")]
    [HarmonyPostfix]
    public static void Interactable_OnInteraction_Postfix(Interactable __instance, InteractablePreset.InteractionAction action, Actor who, bool isInteractionSuccess)
    {
        if (who is Player && isInteractionSuccess)
        {
            Plugin.Log.LogInfo($"Player interacted with: {__instance.name} - Action: {action?.interactionName}");
        }
    }

    /// <summary>
    /// Called when an object is picked up
    /// </summary>
    [HarmonyPatch(typeof(Interactable), "PickUpTarget")]
    [HarmonyPostfix]
    public static void Interactable_PickUp_Postfix(Interactable __instance, Human pickerUpper, bool __result)
    {
        if (__result && pickerUpper is Player)
        {
            Plugin.Log.LogInfo($"Player picked up: {__instance.name}");
        }
    }

    /// <summary>
    /// Hook lockpicking completion
    /// </summary>
    [HarmonyPatch(typeof(Interactable), "OnCompleteLockpick")]
    [HarmonyPostfix]
    public static void Interactable_OnCompleteLockpick_Postfix(Interactable __instance)
    {
        Plugin.Log.LogInfo($"Lockpicked: {__instance.name}");
    }
}

/// <summary>
/// Patches for FirstPersonItemController - handles player inventory and held items
/// FirstPersonItemController fields:
///   - currentItem (int) - current slot index
///   - slots - InventorySlot array
///   - isAiming (bool)
///   - equippedObject (Interactable)
/// </summary>
[HarmonyPatch]
public static class InventoryPatches
{
    [HarmonyPatch(typeof(FirstPersonItemController), "SetSlot")]
    [HarmonyPostfix]
    public static void FPItem_SetSlot_Postfix(FirstPersonItemController __instance, int newSlot)
    {
        // Plugin.Log.LogInfo($"Switched to slot: {newSlot}");
    }
}
