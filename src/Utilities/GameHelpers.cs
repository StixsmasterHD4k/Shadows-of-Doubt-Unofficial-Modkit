using UnityEngine;
using System.Collections.Generic;
using Il2CppSystem.Collections.Generic;

namespace ShadowsOfDoubtMod.Utilities;

/// <summary>
/// Helper utilities for accessing game systems
/// </summary>
public static class GameHelpers
{
    #region Singletons Access
    
    /// <summary>Get the Player instance</summary>
    public static Player GetPlayer() => Player.Instance;
    
    /// <summary>Get the SessionData instance (game time, pause, weather)</summary>
    public static SessionData GetSessionData() => SessionData.Instance;
    
    /// <summary>Get the CityData instance (city generation data)</summary>
    public static CityData GetCityData() => CityData.Instance;
    
    /// <summary>Get the GameplayController instance (money, evidence, cases)</summary>
    public static GameplayController GetGameplayController() => GameplayController.Instance;
    
    /// <summary>Get the MurderController instance</summary>
    public static MurderController GetMurderController() => MurderController.Instance;
    
    /// <summary>Get the AudioController instance</summary>
    public static AudioController GetAudioController() => AudioController.Instance;
    
    /// <summary>Get the InputController instance</summary>
    public static InputController GetInputController() => InputController.Instance;
    
    /// <summary>Get the InterfaceController instance (UI management)</summary>
    public static InterfaceController GetInterfaceController() => InterfaceController.Instance;
    
    /// <summary>Get CityBuildings for building lookup</summary>
    public static CityBuildings GetCityBuildings() => CityBuildings.Instance;
    
    /// <summary>Get CityDistricts for district lookup</summary>  
    public static CityDistricts GetCityDistricts() => CityDistricts.Instance;
    
    #endregion

    #region Player Utilities
    
    /// <summary>Add money to the player</summary>
    public static void AddMoney(int amount, string message = "")
    {
        var gc = GetGameplayController();
        if (gc != null) gc.AddMoney(amount, !string.IsNullOrEmpty(message), message);
    }

    /// <summary>Get player's current money</summary>
    public static int GetMoney()
    {
        var gc = GetGameplayController();
        return gc?.money ?? 0;
    }

    /// <summary>Get player's current health</summary>
    public static float GetPlayerHealth()
    {
        var player = GetPlayer();
        return player?.health ?? 0f;
    }

    /// <summary>Set player's health</summary>
    public static void SetPlayerHealth(float health)
    {
        var player = GetPlayer();
        if (player != null) player.health = Mathf.Clamp(health, 0f, player.maxHealth);
    }

    /// <summary>Heal the player</summary>
    public static void HealPlayer(float amount)
    {
        var player = GetPlayer();
        if (player != null) player.Heal(amount);
    }

    /// <summary>Get player's current position</summary>
    public static Vector3 GetPlayerPosition()
    {
        var player = GetPlayer();
        return player?.transform.position ?? Vector3.zero;
    }

    /// <summary>Teleport player to position</summary>
    public static void TeleportPlayer(Vector3 position)
    {
        var player = GetPlayer();
        if (player?.charController != null)
        {
            player.charController.enabled = false;
            player.transform.position = position;
            player.charController.enabled = true;
        }
    }

    #endregion

    #region Time Utilities
    
    /// <summary>Get current in-game time (0-24 hours)</summary>
    public static float GetGameTime()
    {
        var session = GetSessionData();
        return session?.gameTime ?? 0f;
    }

    /// <summary>Get current day number</summary>
    public static int GetCurrentDay()
    {
        var session = GetSessionData();
        return session?.day ?? 0;
    }

    /// <summary>Pause the game</summary>
    public static void PauseGame()
    {
        var session = GetSessionData();
        session?.PauseGame(true, false, true);
    }

    /// <summary>Resume the game</summary>
    public static void ResumeGame()
    {
        var session = GetSessionData();
        session?.ResumeGame();
    }

    /// <summary>Check if game is paused</summary>
    public static bool IsPaused()
    {
        var session = GetSessionData();
        return session != null && !session.play;
    }

    #endregion

    #region Citizen Utilities
    
    /// <summary>Find a citizen by name</summary>
    public static Human FindCitizenByName(string name)
    {
        var cityData = GetCityData();
        if (cityData?.citizenDictionary == null) return null;
        
        foreach (var kvp in cityData.citizenDictionary)
        {
            if (kvp.Value?.citizenName?.Contains(name) == true)
                return kvp.Value;
        }
        return null;
    }

    /// <summary>Get all citizens in the city</summary>
    public static System.Collections.Generic.List<Human> GetAllCitizens()
    {
        var result = new System.Collections.Generic.List<Human>();
        var cityData = GetCityData();
        if (cityData?.citizenDictionary == null) return result;
        
        foreach (var kvp in cityData.citizenDictionary)
        {
            if (kvp.Value != null) result.Add(kvp.Value);
        }
        return result;
    }

    #endregion

    #region Case/Murder Utilities
    
    /// <summary>Get the active murder case</summary>
    public static MurderController.Murder GetActiveMurder()
    {
        var mc = GetMurderController();
        return mc?.activeMurder;
    }

    /// <summary>Get the murder victim</summary>
    public static Human GetMurderVictim()
    {
        return GetActiveMurder()?.victim;
    }

    /// <summary>Get the murderer</summary>
    public static Human GetMurderer()
    {
        return GetActiveMurder()?.murderer;
    }

    #endregion
}
