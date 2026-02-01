using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace ShadowsOfDoubtMod;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;
    private Harmony _harmony;

    public override void Load()
    {
        Log = base.Log;
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        // Register custom MonoBehaviour types if needed
        // ClassInjector.RegisterTypeInIl2Cpp<MyCustomComponent>();

        // Apply Harmony patches
        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        _harmony.PatchAll();

        // Add Unity component to handle Update loop
        AddComponent<ModController>();
    }

    public override bool Unload()
    {
        _harmony?.UnpatchSelf();
        return base.Unload();
    }
}

// MonoBehaviour for handling Unity callbacks
public class ModController : MonoBehaviour
{
    void Update()
    {
        // Handle input, etc.
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Plugin.Log.LogInfo("F5 pressed - custom action triggered!");
            // Add your custom functionality here
        }
    }
}
