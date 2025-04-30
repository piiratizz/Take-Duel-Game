using Mirror;
using Steamworks;
using UnityEngine;
using Zenject;

public class LobbyService : MonoBehaviour
{
    [Inject] private CustomNetworkManager _networkManager;

    private Callback<LobbyCreated_t> _lobbyCreatedCallback;
    private Callback<GameLobbyJoinRequested_t> _lobbyJoinRequestedCallback;
    private Callback<LobbyEnter_t> _lobbyEnteredCallback;

    private const string HostAddressKey = "HostAddress";
    
    public void Initialize()
    {
        if(!SteamManager.Initialized) return;

        _lobbyCreatedCallback = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        _lobbyJoinRequestedCallback = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyJoinRequested);
        _lobbyEnteredCallback = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, _networkManager.maxConnections);
    }
    
    private void OnLobbyEntered(LobbyEnter_t param)
    {
        if (NetworkServer.active)
        {
            return;
        }

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(param.m_ulSteamIDLobby), HostAddressKey);

        _networkManager.networkAddress = hostAddress;
        _networkManager.StartClient();
    }

    private void OnLobbyJoinRequested(GameLobbyJoinRequested_t param)
    {
        SteamMatchmaking.JoinLobby(param.m_steamIDLobby);
    }

    private void OnLobbyCreated(LobbyCreated_t param)
    {
        if (param.m_eResult != EResult.k_EResultOK)
        {
            return;
        }
        
        _networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(
            new CSteamID(param.m_ulSteamIDLobby),
            HostAddressKey,
            SteamUser.GetSteamID().ToString());
    }
}
