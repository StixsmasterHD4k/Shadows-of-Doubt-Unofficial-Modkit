using BepInEx.Configuration;

namespace ShadowsOfDoubtMod.Config;

/// <summary>
/// Configuration for the mod - uses BepInEx config system
/// Config file will be created at BepInEx/config/com.yourname.shadowsofdoubtmod.cfg
/// </summary>
public static class ModConfig
{
    public static ConfigEntry<bool> EnableDebugMode { get; private set; }
    public static ConfigEntry<bool> EnableGodMode { get; private set; }
    public static ConfigEntry<float> MoneyMultiplier { get; private set; }
    public static ConfigEntry<KeyCode> DebugMenuKey { get; private set; }

    public static void Initialize(ConfigFile config)
    {
        EnableDebugMode = config.Bind(
            "General",
            "EnableDebugMode",
            false,
            "Enable debug logging and debug commands"
        );

        EnableGodMode = config.Bind(
            "Cheats",
            "EnableGodMode", 
            false,
            "Player takes no damage"
        );

        MoneyMultiplier = config.Bind(
            "Gameplay",
            "MoneyMultiplier",
            1.0f,
            new ConfigDescription(
                "Multiplier for money rewards",
                new AcceptableValueRange<float>(0.1f, 10f)
            )
        );

        DebugMenuKey = config.Bind(
            "Keybinds",
            "DebugMenuKey",
            KeyCode.F10,
            "Key to open debug menu"
        );
    }
}
