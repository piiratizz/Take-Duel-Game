using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Steamworks;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class SteamManager : MonoBehaviour
{
    private Callback<LobbyMatchList_t> _onLobbyListReceived;
    private Callback<LobbyDataUpdate_t> _onLobbyDataUpdated;
    private List<CSteamID> _foundLobbies = new();
    public UnityEvent<LobbyData> LobbyDataReceived;
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
            
            _onLobbyListReceived = Callback<LobbyMatchList_t>.Create(OnLobbyListReceived);
            _onLobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdated);
            LobbyDataReceived = new UnityEvent<LobbyData>();
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

    public void RequestLobbiesList()
    {
        SteamMatchmaking.AddRequestLobbyListResultCountFilter(5000);
        SteamMatchmaking.RequestLobbyList();
    }
    
    private void OnLobbyDataUpdated(LobbyDataUpdate_t param)
    {
        if (param.m_bSuccess != 1) return;

        var lobbyID = (CSteamID)param.m_ulSteamIDLobby;
        var roomName = SteamMatchmaking.GetLobbyData(lobbyID, "name");
        
        var data = new LobbyData()
        {
            LobbyId = lobbyID,
            RoomName = roomName
        };

        LobbyDataReceived.Invoke(data);
    }

    private void OnLobbyListReceived(LobbyMatchList_t param)
    {
        _foundLobbies.Clear();
        for (int i = 0; i < param.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyId = SteamMatchmaking.GetLobbyByIndex(i);
            _foundLobbies.Add(lobbyId);
            SteamMatchmaking.RequestLobbyData(lobbyId); 
        }
    }
    
    void OnApplicationQuit()
    {
        if (_initialized)
        {
            SteamAPI.Shutdown();
        }
    }
}