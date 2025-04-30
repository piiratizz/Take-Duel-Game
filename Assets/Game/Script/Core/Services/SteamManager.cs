using System;
using UnityEngine;
using Steamworks;

public class SteamManager : MonoBehaviour
{
    public static bool Initialized => _initialized;
    private static bool _initialized;

    public void Initialize()
    {
        if (!_initialized)
        {
            if (!SteamAPI.Init())
            {
                Debug.LogError("SteamAPI init failed!");
                return;
            }
            
            Debug.Log($"Connected to steam profile: {SteamFriends.GetPersonaName()}");
            _initialized = true;
        }
    }

    private void Update()
    {
        if(!_initialized) return;
        
        SteamAPI.RunCallbacks();
    }

    void OnApplicationQuit()
    {
        if (_initialized)
        {
            SteamAPI.Shutdown();
        }
    }
}