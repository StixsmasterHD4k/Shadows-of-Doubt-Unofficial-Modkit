# Shadows of Doubt BepInEx 6+ Mod Kit

Complete modding kit for **Shadows of Doubt** using BepInEx 6 (Unity IL2CPP).

## Contents

```
ShadowsOfDoubtModKit/
├── src/                          # Complete mod template
│   ├── Plugin.cs                 # Main plugin entry
│   ├── ShadowsOfDoubtMod.csproj  # Project file
│   ├── Config/ModConfig.cs       # Configuration system
│   ├── Patches/                  # Harmony patches
│   │   ├── PlayerPatches.cs
│   │   ├── GameplayPatches.cs
│   │   ├── CasePatches.cs
│   │   ├── CitizenPatches.cs
│   │   └── InteractablePatches.cs
│   └── Utilities/
│       ├── GameHelpers.cs        # Helper functions
│       └── DebugCommands.cs      # Debug/cheat commands
├── lib/
│   └── DummyDll/                 # Reference assemblies (from Il2CppDumper)
├── docs/
│   ├── SETUP.md                  # Installation guide
│   ├── GAME_CLASSES.md           # Game class reference
│   ├── HARMONY_EXAMPLES.md       # Patching examples
│   └── dump.cs                   # Full decompiled source (~778K lines)
└── examples/
    ├── MoneyMod.cs               # Simple money multiplier
    └── TeleportMod.cs            # Position save/teleport
```

## Quick Start

1. **Install BepInEx 6 Bleeding Edge** (Unity IL2CPP version)
   - Download: https://builds.bepinex.dev/projects/bepinex_be
   - Extract to Shadows of Doubt game folder
   - Run game once to generate interop assemblies

2. **Setup Project**
   - Copy `src/` to your project folder
   - Edit `.csproj` and set `$(GameDir)` to your game path
   - Build with `dotnet build`

3. **Install Mod**
   - Copy built DLL to `BepInEx/plugins/`
   - Launch game

## Key Game Classes

| Class | Access | Purpose |
|-------|--------|---------|
| `Player` | `Player.Instance` | Player character |
| `GameplayController` | `GameplayController.Instance` | Money, evidence, cases |
| `SessionData` | `SessionData.Instance` | Time, pause, weather |
| `MurderController` | `MurderController.Instance` | Murder system |
| `CityData` | `CityData.Instance` | Citizens, buildings |

## Common Hooks

```csharp
// Double money
[HarmonyPatch(typeof(GameplayController), "AddMoney")]
[HarmonyPrefix]
public static void AddMoney_Prefix(ref int amount) => amount *= 2;

// God mode
[HarmonyPatch(typeof(Human), "RecieveDamage")]
[HarmonyPrefix]
public static bool RecieveDamage_Prefix(Human __instance) 
    => !(__instance is Player); // false = skip original

// Track evidence
[HarmonyPatch(typeof(Evidence), "Discover")]
[HarmonyPostfix]
public static void Evidence_Discover(Evidence __instance)
    => Plugin.Log.LogInfo($"Found: {__instance.evName}");
```

## Game Info

- **Engine**: Unity 2021 (IL2CPP)
- **IL2CPP Version**: 31 (runtime 29)
- **Main Assembly**: Assembly-CSharp.dll (~4MB)
- **Total Classes**: ~2600+
- **Decompiled Lines**: 778,176

## Requirements

- .NET 6.0 SDK
- BepInEx 6 Unity IL2CPP (Bleeding Edge)
- Shadows of Doubt (Steam)

## Resources

- [BepInEx Documentation](https://docs.bepinex.dev/)
- [Harmony Wiki](https://harmony.pardeike.net/)
- [Il2CppInterop](https://github.com/BepInEx/Il2CppInterop)

## License

MIT - Free to use, modify, distribute.
