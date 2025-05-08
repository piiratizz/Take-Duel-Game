using Zenject;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class CustomNetworkManager : NetworkManager
{
    [Header("CUSTOM SECTION")] 
    [SerializeField] private GameObject _playerPrefab;
    [Inject] private DiContainer _container;
    private GameObject _lastSpawnedPlayer;

    [HideInInspector] public UnityEvent<NetworkConnectionToClient> PlayerConnectedEvent;
    [HideInInspector] public UnityEvent<NetworkConnectionToClient> PlayerDisconnectedEvent;
    [HideInInspector] public UnityEvent<NetworkConnectionToClient> PlayerSpawnedEvent;
    [HideInInspector] public UnityEvent ClientDisconnectedEvent;

    public override void Awake()
    {
        base.Awake();
        PlayerConnectedEvent = new UnityEvent<NetworkConnectionToClient>();
        PlayerDisconnectedEvent = new UnityEvent<NetworkConnectionToClient>();
        PlayerSpawnedEvent = new UnityEvent<NetworkConnectionToClient>();
        ClientDisconnectedEvent = new UnityEvent();
    }
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player = Instantiate(
            _playerPrefab,
            Vector3.zero, 
            Quaternion.identity,
            null);
        _lastSpawnedPlayer = player;
        
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

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        ClientDisconnectedEvent.Invoke();
    }
}