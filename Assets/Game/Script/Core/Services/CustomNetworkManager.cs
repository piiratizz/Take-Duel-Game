using System.Collections.Generic;
using Zenject;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class CustomNetworkManager : NetworkManager
{
    [Header("CUSTOM SECTION")] 
    [SerializeField] private GameObject _playerPrefab;

    [HideInInspector] public UnityEvent<NetworkConnectionToClient> PlayerConnectedEvent;
    [HideInInspector] public UnityEvent<NetworkConnectionToClient> PlayerDisconnectedEvent;
    [HideInInspector] public UnityEvent<NetworkConnectionToClient> PlayerSpawnedEvent;
    [HideInInspector] public UnityEvent ClientDisconnectedEvent;

    [Inject] private PlayerDataStorageService _playerDataStorageService;
    [Inject] private SteamManager _steamManager;
    
    private Dictionary<NetworkConnectionToClient, PlayerDataMessage> _playersData;

    public PlayerDataMessage GetPlayerData(NetworkConnectionToClient playerConnection) =>
        _playersData[playerConnection];
    
    public override void Awake()
    {
        base.Awake();
        PlayerConnectedEvent = new UnityEvent<NetworkConnectionToClient>();
        PlayerDisconnectedEvent = new UnityEvent<NetworkConnectionToClient>();
        PlayerSpawnedEvent = new UnityEvent<NetworkConnectionToClient>();
        ClientDisconnectedEvent = new UnityEvent();
        _playersData = new Dictionary<NetworkConnectionToClient, PlayerDataMessage>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<PlayerDataMessage>(OnReceivePlayerData);
    }

    private void OnReceivePlayerData(NetworkConnectionToClient conn, PlayerDataMessage message)
    {
        _playersData.Add(conn, message);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player = Instantiate(
            _playerPrefab,
            Vector3.zero, 
            Quaternion.identity,
            null);
        
        NetworkServer.AddPlayerForConnection(conn, player);
        PlayerSpawnedEvent.Invoke(conn);
        Debug.Log("PLAYER SPAWNED");
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        PlayerDisconnectedEvent.Invoke(conn);
        Debug.Log($"Player disconnected: {conn.connectionId}");
    }
    
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        Debug.Log($"Player connected: {conn.connectionId}");
        PlayerConnectedEvent.Invoke(conn);
    }

    public override async void OnClientConnect()
    {
        base.OnClientConnect();
        var data = _playerDataStorageService.Data;
        var msg = new PlayerDataMessage()
        {
            SkinName = data.SelectedSkin,
            Nickname = _steamManager.GetPlayerName(),
            AvatarInt = await _steamManager.GetPlayerAvatarIntAsync()
        };
        NetworkClient.Send(msg);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        ClientDisconnectedEvent.Invoke();
    }
}