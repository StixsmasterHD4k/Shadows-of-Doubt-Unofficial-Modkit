# Harmony Patching Examples for Shadows of Doubt

## Harmony Patch Types

### Prefix Patch
Runs BEFORE the original method. Can skip original by returning `false`.

```csharp
[HarmonyPatch(typeof(ClassName), "MethodName")]
[HarmonyPrefix]
public static bool MethodName_Prefix(ClassName __instance, ref ParameterType param)
{
    // __instance = the object instance
    // ref param = can modify parameter
    // return false to skip original method
    return true;
}
```

### Postfix Patch
Runs AFTER the original method.

```csharp
[HarmonyPatch(typeof(ClassName), "MethodName")]
[HarmonyPostfix]
public static void MethodName_Postfix(ClassName __instance, ref ReturnType __result)
{
    // __result = return value (can modify)
}
```

### Transpiler Patch
Modifies IL code directly (advanced).

---

## Common Patches

### Modify Money Gains
```csharp
[HarmonyPatch(typeof(GameplayController), "AddMoney")]
[HarmonyPrefix]
public static void AddMoney_Prefix(ref int amount)
{
    amount *= 2; // Double all money gains
}
```

### God Mode
```csharp
[HarmonyPatch(typeof(Human), "RecieveDamage")]
[HarmonyPrefix]
public static bool RecieveDamage_Prefix(Human __instance)
{
    if (__instance is Player && GodModeEnabled)
        return false; // Skip damage for player
    return true;
}
```

### Infinite Lockpicks
```csharp
[HarmonyPatch(typeof(Interactable), "OnCompleteLockpick")]
[HarmonyPrefix]
public static void OnCompleteLockpick_Prefix()
{
    var gc = GameplayController.Instance;
    if (gc != null) gc.lockPicks++; // Add one to compensate for use
}
```

### Log All Evidence Discovery
```csharp
[HarmonyPatch(typeof(Evidence), "Discover")]
[HarmonyPostfix]
public static void Evidence_Discover(Evidence __instance)
{
    Plugin.Log.LogInfo($"Found evidence: {__instance.evName}");
}
```

### Speed Up Time
```csharp
[HarmonyPatch(typeof(SessionData), "Update")]
[HarmonyPostfix]
public static void SessionData_Update(SessionData __instance)
{
    if (SpeedUpTime)
        __instance.gameTime += Time.deltaTime * 0.01f; // Extra time
}
```

### Modify Case Rewards
```csharp
[HarmonyPatch(typeof(Case), "CalculateReward")]
[HarmonyPostfix]
public static void Case_CalculateReward(Case __instance, ref int __result)
{
    __result = (int)(__result * 1.5f); // 50% more rewards
}
```

### Track Player Location
```csharp
[HarmonyPatch(typeof(Player), "OnGameLocationChange")]
[HarmonyPostfix]
public static void Player_OnGameLocationChange(Player __instance)
{
    var loc = __instance.currentGameLocation;
    if (loc != null)
    {
        Plugin.Log.LogInfo($"Player entered: {loc.name}");
        // Store for fast travel, etc.
    }
}
```

### Hook Conversations
```csharp
[HarmonyPatch(typeof(Human), "StartConversation")]
[HarmonyPostfix]
public static void Human_StartConversation(Human __instance, Human other)
{
    if (__instance is Player && other != null)
    {
        // Log or modify conversation
        Plugin.Log.LogInfo($"Talking to: {other.citizenName}");
    }
}
```

### Reveal All Fingerprints
```csharp
[HarmonyPatch(typeof(Interactable), "OnInteraction")]
[HarmonyPostfix]
public static void Interactable_OnInteraction(Interactable __instance, Actor who)
{
    if (who is Player && RevealFingerprints)
    {
        // Auto-check for fingerprints
        if (__instance.df != null && __instance.df.Count > 0)
        {
            Plugin.Log.LogInfo($"Object has {__instance.df.Count} fingerprints");
        }
    }
}
```

### Hook Murder State Changes
```csharp
[HarmonyPatch(typeof(MurderController), "SetMurderState")]
[HarmonyPostfix]
public static void Murder_StateChange(MurderController.Murder murder, MurderController.MurderState newState)
{
    if (newState == MurderController.MurderState.executing)
    {
        Plugin.Log.LogInfo("Murder is happening NOW!");
        // Could trigger alert, marker, etc.
    }
}
```

---

## IL2CPP-Specific Notes

### Accessing Il2Cpp Lists
```csharp
// Il2Cpp lists need to be iterated differently
var citizens = CityData.Instance.citizenDictionary;
foreach (var kvp in citizens)
{
    Human citizen = kvp.Value;
    // Process citizen
}
```

### Checking Types
```csharp
// Use 'is' for type checking
if (actor is Player player)
{
    // Use player
}
```

### Null Safety
```csharp
// Always null-check IL2CPP objects
if (instance?.field != null)
{
    // Safe to use
}
```

### Creating Delegates for Events
```csharp
// For IL2CPP events, use Il2CppSystem delegates
using Il2CppSystem;

// Subscribe to event
instance.OnSomeEvent += (Action)MyHandler;

void MyHandler()
{
    // Handle event
}
```
