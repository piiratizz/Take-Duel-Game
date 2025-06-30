using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class LobbyService : NetworkBehaviour
{
    [Inject] private CustomNetworkManager _networkManager;
    [Inject] private SceneService _sceneService;
    [Inject] private SteamManager _steamManager;

    private Callback<LobbyCreated_t> _lobbyCreatedCallback;
    private Callback<GameLobbyJoinRequested_t> _lobbyJoinRequestedCallback;
    private Callback<LobbyEnter_t> _lobbyEnteredCallback;
    private Callback<LobbyChatUpdate_t> _lobbyChatUpdate;

    private const string HostAddressKey = "HostAddress";

    public UnityEvent<LobbyData> LobbyCreatedEvent;
    public UnityEvent<List<LobbyPlayerData>> PlayerListUpdatedEvent;
    public UnityEvent<LobbyData> LobbySingleJoinedEvent;

    private bool _lobbyCreated;

    public void Initialize()
    {
        if (!SteamManager.Initialized) return;
        _lobbyCreatedCallback = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        _lobbyJoinRequestedCallback = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyJoinRequested);
        _lobbyEnteredCallback = Callback<LobbyEnter_t>.Create(OnLobbyEnteredSingle);
        _lobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnNewPlayerEntered);

        LobbyCreatedEvent = new UnityEvent<LobbyData>();
        PlayerListUpdatedEvent = new UnityEvent<List<LobbyPlayerData>>();
        LobbySingleJoinedEvent = new UnityEvent<LobbyData>();
    }


    public void CreateLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, _networkManager.maxConnections);
    }

    public void LeaveLobby(CSteamID lobbyId)
    {
        Debug.Log($"Lobby leaved");
        SteamMatchmaking.LeaveLobby(lobbyId);
        _lobbyCreated = false;

        if (NetworkServer.active)
        {
            _networkManager.StopHost();
        }
        else
        {
            _networkManager.StopClient();
        }
    }

    public void JoinLobby(CSteamID lobbyId)
    {
        SteamMatchmaking.JoinLobby(lobbyId);
    }

    private void OnLobbyEnteredSingle(LobbyEnter_t param)
    {
        var lobbyId = new CSteamID(param.m_ulSteamIDLobby);

        if (NetworkServer.active)
        {
            RefreshPlayersList(lobbyId);
            return;
        }

        var hostAddress = SteamMatchmaking.GetLobbyData(lobbyId, HostAddressKey);
        _networkManager.networkAddress = hostAddress;

        LobbySingleJoinedEvent.Invoke(new LobbyData()
        {
            LobbyId = lobbyId,
            RoomName = SteamMatchmaking.GetLobbyData(lobbyId, "hostName")
        });

        RefreshPlayersList(lobbyId);
        _networkManager.StartClient();
    }

    private void OnNewPlayerEntered(LobbyChatUpdate_t param)
    {
        var lobbyId = (CSteamID)param.m_ulSteamIDLobby;
        var changedUser = (CSteamID)param.m_ulSteamIDUserChanged;
        var change = (EChatMemberStateChange)param.m_rgfChatMemberStateChange;

        Debug.Log($"LobbyChatUpdate: {change} from {changedUser}");

        RefreshPlayersList(lobbyId);
    }

    private void RefreshPlayersList(CSteamID lobbyId)
    {
        var memberCount = SteamMatchmaking.GetNumLobbyMembers(lobbyId);

        var players = new List<LobbyPlayerData>();

        for (int i = 0; i < memberCount; i++)
        {
            var memberID = SteamMatchmaking.GetLobbyMemberByIndex(lobbyId, i);
            var nickname = SteamFriends.GetFriendPersonaName(memberID);

            players.Add(new LobbyPlayerData()
            {
                Nickname = nickname,
            });
        }

        PlayerListUpdatedEvent.Invoke(players);
    }

    private void OnLobbyJoinRequested(GameLobbyJoinRequested_t param)
    {
        SteamMatchmaking.JoinLobby(param.m_steamIDLobby);
    }

    private async void OnLobbyCreated(LobbyCreated_t param)
    {
        if (_lobbyCreated) return;
        _lobbyCreated = true;

        Debug.Log($"Lobby created with status: {param.m_eResult}");
        if (param.m_eResult != EResult.k_EResultOK)
        {
            return;
        }

        var lobbyId = new CSteamID(param.m_ulSteamIDLobby);

        SteamMatchmaking.SetLobbyData(
            lobbyId,
            HostAddressKey,
            SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(
            lobbyId,
            "hostName",
            SteamFriends.GetPersonaName());

        LobbyCreatedEvent.Invoke(new LobbyData()
        {
            LobbyId = lobbyId,
            RoomName = SteamMatchmaking.GetLobbyData(lobbyId, "hostName")
        });

        Debug.Log("START HOSTING");
        _networkManager.StartHost();
    }
    
    [Server]
    public void StartGame()
    {
        //await _sceneService.LoadGameplayAsync();
        //_networkManager.Spawner.CmdRequestSpawnPlayer();
        _networkManager.ServerChangeScene(Scenes.Gameplay);
    }
}