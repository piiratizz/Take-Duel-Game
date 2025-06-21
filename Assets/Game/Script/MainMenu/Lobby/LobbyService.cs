using Cysharp.Threading.Tasks;
using Mirror;
using Steamworks;
using UnityEngine;
using Zenject;

public class LobbyService : NetworkBehaviour
{
    [Inject] private CustomNetworkManager _networkManager;
    [Inject] private SceneService _sceneService;

    private Callback<LobbyCreated_t> _lobbyCreatedCallback;
    private Callback<GameLobbyJoinRequested_t> _lobbyJoinRequestedCallback;
    private Callback<LobbyEnter_t> _lobbyEnteredCallback;

    private const string HostAddressKey = "HostAddress";

    public void Initialize()
    {
        if (!SteamManager.Initialized) return;
        _lobbyCreatedCallback = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        _lobbyJoinRequestedCallback = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyJoinRequested);
        _lobbyEnteredCallback = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public async void CreateLobby()
    {
        //await StartHost();
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, _networkManager.maxConnections);
    }

    public void JoinLobby(CSteamID lobbyId)
    {
        SteamMatchmaking.JoinLobby(lobbyId);
    }

    private void OnLobbyEntered(LobbyEnter_t param)
    {
        if (NetworkServer.active)
        {
            return;
        }
        
        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(param.m_ulSteamIDLobby), HostAddressKey);
        
        _networkManager.networkAddress = hostAddress;

        //StartClient();
    }

    private async UniTask StartClient()
    {
        await _sceneService.LoadGameplayAsync();
        _networkManager.StartClient();
    }

    private async UniTask StartHost()
    {
        await _sceneService.LoadGameplayAsync();
        _networkManager.StartHost();
    }

    private void OnLobbyJoinRequested(GameLobbyJoinRequested_t param)
    {
        SteamMatchmaking.JoinLobby(param.m_steamIDLobby);
    }

    private void OnLobbyCreated(LobbyCreated_t param)
    {
        Debug.Log($"Lobby created with status: {param.m_eResult}");
        if (param.m_eResult != EResult.k_EResultOK)
        {
            return;
        }

        SteamMatchmaking.SetLobbyData(
            new CSteamID(param.m_ulSteamIDLobby),
            HostAddressKey,
            SteamUser.GetSteamID().ToString());
    }
}