using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace MoneyMod;

/// <summary>
/// Simple example mod that doubles all money gains
/// </summary>
[BepInPlugin("com.example.moneymod", "Money Modifier", "1.0.0")]
public class MoneyModPlugin : BasePlugin
{
    internal static ManualLogSource Logger;
    public static ConfigEntry<float> MoneyMultiplier;

    public override void Load()
    {
        Logger = Log;
        
        MoneyMultiplier = Config.Bind(
            "General",
            "MoneyMultiplier",
            2.0f,
            "Multiplier for all money gains"
        );

        var harmony = new Harmony("com.example.moneymod");
        harmony.PatchAll();

        Log.LogInfo($"Money Mod loaded! Multiplier: {MoneyMultiplier.Value}x");
    }
}

[HarmonyPatch]
public static class MoneyPatches
{
    [HarmonyPatch(typeof(GameplayController), "AddMoney")]
    [HarmonyPrefix]
    public static void AddMoney_Prefix(ref int amount, bool displayMessage)
    {
        if (amount > 0) // Only positive gains
        {
            int original = amount;
            amount = (int)(amount * MoneyModPlugin.MoneyMultiplier.Value);
            MoneyModPlugin.Logger.LogInfo($"Money: {original} -> {amount}");
        }
    }
}
