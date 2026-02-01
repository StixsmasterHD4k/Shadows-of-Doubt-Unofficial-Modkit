# Shadows of Doubt Modding Setup Guide

## Prerequisites

1. **Shadows of Doubt** installed via Steam
2. **.NET 6.0 SDK** or later - [Download](https://dotnet.microsoft.com/download)
3. **BepInEx 6 (Bleeding Edge)** for Unity IL2CPP

## Step 1: Install BepInEx 6

### Download BepInEx 6 BE (Bleeding Edge)
Download the latest BepInEx 6 Unity IL2CPP build:
- Go to: https://builds.bepinex.dev/projects/bepinex_be
- Download `BepInEx_Unity.IL2CPP_win_x64_*.zip`

### Install BepInEx
1. Locate your Shadows of Doubt installation:
   - Steam: Right-click game → Manage → Browse local files
   - Default: `C:\Program Files (x86)\Steam\steamapps\common\Shadows of Doubt`

2. Extract BepInEx zip contents into the game folder:
   ```
   Shadows of Doubt/
   ├── BepInEx/           <- Extract here
   ├── Shadows of Doubt_Data/
   ├── Shadows of Doubt.exe
   ├── doorstop_config.ini <- Extract here
   └── winhttp.dll         <- Extract here
   ```

3. Run the game once to generate BepInEx folders and interop assemblies

4. After first run, you'll have:
   ```
   Shadows of Doubt/
   └── BepInEx/
       ├── cache/
       ├── config/
       ├── core/
       ├── interop/        <- Game assemblies for reference
       ├── patchers/
       └── plugins/        <- Put your mods here
   ```

## Step 2: Setup Development Environment

### Create Project
1. Create a new folder for your mod project
2. Copy the contents of `src/` from this kit into your project
3. Update the `.csproj` file:
   - Set `$(GameDir)` to your Shadows of Doubt installation path
   - Or create a `Directory.Build.props` file:
     ```xml
     <Project>
       <PropertyGroup>
         <GameDir>C:\Program Files (x86)\Steam\steamapps\common\Shadows of Doubt</GameDir>
       </PropertyGroup>
     </Project>
     ```

### Build the Project
```bash
dotnet build
```

### Copy to Game
Copy the built DLL from `bin/Debug/net6.0/` to:
```
Shadows of Doubt/BepInEx/plugins/ShadowsOfDoubtMod.dll
```

## Step 3: Testing

1. Launch Shadows of Doubt
2. Check `BepInEx/LogOutput.log` for your plugin's log messages
3. Press F5 in-game to test the example functionality

## Project Structure

```
ShadowsOfDoubtMod/
├── src/
│   ├── Plugin.cs           # Main plugin entry point
│   ├── ShadowsOfDoubtMod.csproj
│   ├── Config/
│   │   └── ModConfig.cs    # Configuration
│   ├── Patches/
│   │   ├── PlayerPatches.cs
│   │   ├── GameplayPatches.cs
│   │   ├── CasePatches.cs
│   │   ├── CitizenPatches.cs
│   │   └── InteractablePatches.cs
│   └── Utilities/
│       ├── GameHelpers.cs   # Helper functions
│       └── DebugCommands.cs # Debug/cheat commands
├── docs/
│   ├── SETUP.md            # This file
│   └── GAME_CLASSES.md     # Game class reference
└── lib/
    └── DummyDlls/          # Reference assemblies from Il2CppDumper
```

## Troubleshooting

### "Assembly not found" errors
- Make sure you've run the game once with BepInEx installed
- Check that `$(GameDir)` is set correctly
- Verify the interop DLLs exist in `BepInEx/interop/`

### Plugin not loading
- Check `BepInEx/LogOutput.log` for errors
- Ensure the DLL is in `BepInEx/plugins/`
- Verify BepInEx is installed correctly (winhttp.dll present)

### Game crashes on start
- Check for Harmony patch errors in the log
- Remove patches one by one to find the culprit
- Some methods may have changed between game versions

## Advanced: Adding NuGet References

If you need additional packages, add them to your `.csproj`:

```xml
<ItemGroup>
  <PackageReference Include="Newtonsoft.Json" Version="13.*" />
</ItemGroup>
```

## Building for Release

```bash
dotnet publish -c Release
```

Create a release zip containing just your plugin DLL and any dependencies.
