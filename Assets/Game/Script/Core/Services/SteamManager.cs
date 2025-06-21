using System;
using Cysharp.Threading.Tasks;
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

    public string GetPlayerName()
    {
        return SteamFriends.GetPersonaName();
    }

    public CSteamID GetPlayerSteamID()
    {
        return SteamUser.GetSteamID();
    }

    public async UniTask<Texture2D> GetPlayerAvatarAsync()
    {
        var avatarInt = await GetPlayerAvatarIntAsync();

        return ConvertAvatarIntToTexture(avatarInt);
    }

    public async UniTask<int> GetPlayerAvatarIntAsync()
    {
        CSteamID steamId = GetPlayerSteamID();
        int avatarInt = 0;
        
        await UniTask.WaitWhile(() =>
        {
            avatarInt = SteamFriends.GetMediumFriendAvatar(steamId);
            return avatarInt == -1;
        });

        if (avatarInt == 0)
        {
            Debug.LogWarning("Cant load avatar");
        }

        return avatarInt;
    }

    public Texture2D ConvertAvatarIntToTexture(int avatarInt)
    {
        uint width, height;
        if (!SteamUtils.GetImageSize(avatarInt, out width, out height) || width == 0 || height == 0)
        {
            Debug.LogWarning("Invalid avatar size");
        }

        byte[] image = new byte[4 * width * height];
        
        if (!SteamUtils.GetImageRGBA(avatarInt, image, image.Length))
        {
            Debug.LogWarning("Cant get avatar");
        }
        
        Texture2D avatarTexture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
        avatarTexture.LoadRawTextureData(image);
        avatarTexture.Apply();

        return avatarTexture;
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