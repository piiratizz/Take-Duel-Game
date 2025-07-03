using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Zenject;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class CustomNetworkManager : NetworkManager
{
    [Header("CUSTOM SECTION")] 
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _lobbyPlayerPrefab;
    
    [HideInInspector] public UnityEvent ClientDisconnectedEvent;

    [Inject] private PlayerDataStorageService _playerDataStorageService;
    [Inject] private SteamManager _steamManager;
    
    private Dictionary<NetworkConnectionToClient, PlayerDataMessage> _playersData;
    private List<NetworkConnectionToClient> _connectedPlayers;

    public PlayerDataMessage GetPlayerData(NetworkConnectionToClient playerConnection) =>
        _playersData[playerConnection];

    public IReadOnlyList<NetworkConnectionToClient> ConnectedPlayer => _connectedPlayers;
    
    public override void Awake()
    {
        base.Awake();
        ClientDisconnectedEvent = new UnityEvent();
        _playersData = new Dictionary<NetworkConnectionToClient, PlayerDataMessage>();
        _connectedPlayers = new List<NetworkConnectionToClient>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<PlayerDataMessage>(OnReceivePlayerData);
    }

    private void OnReceivePlayerData(NetworkConnectionToClient conn, PlayerDataMessage message)
    {
        Debug.Log($"Message received {message.Nickname}");
        _playersData.Add(conn, message);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        //base.OnServerAddPlayer(conn);
        GameObject player = Instantiate(
            _lobbyPlayerPrefab,
            Vector3.zero, 
            Quaternion.identity,
            null);
        
        NetworkServer.AddPlayerForConnection(conn, player);
        Debug.Log("PLAYER SPAWNED");
    }
    
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log($"Player disconnected: {conn.connectionId}");
    }
    
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        Debug.Log($"Player connected: {conn.connectionId}");
        _connectedPlayers.Add(conn);
    }

    public override async void OnClientConnect()
    {
        base.OnClientConnect();
        var data = _playerDataStorageService.Data;
        var msg = new PlayerDataMessage()
        {
            SkinName = data.SelectedSkin,
            Nickname = _steamManager.GetPlayerName(),
        };
        
        NetworkClient.Send(msg);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        ClientDisconnectedEvent.Invoke();
    }
    
    public override async void OnServerSceneChanged(string sceneName)
    {
        if (sceneName == Scenes.Gameplay)
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                await UniTask.WaitWhile(() => !conn.isReady);
                var newPlayer = Instantiate(_playerPrefab);
                var oldPlayer = conn.identity.gameObject;
                //NetworkServer.Destroy(conn.identity.gameObject);
                NetworkServer.ReplacePlayerForConnection(conn, newPlayer, ReplacePlayerOptions.KeepAuthority);
                Destroy(oldPlayer, 0.1f);
            }
        }
    }
    
}