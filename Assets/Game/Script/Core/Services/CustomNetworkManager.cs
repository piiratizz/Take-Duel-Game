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

    public override void Awake()
    {
        base.Awake();
        PlayerConnectedEvent = new UnityEvent<NetworkConnectionToClient>();
        PlayerDisconnectedEvent = new UnityEvent<NetworkConnectionToClient>();
        PlayerSpawnedEvent = new UnityEvent<NetworkConnectionToClient>();
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
        Debug.Log($"Player disconnected: {conn.connectionId}");
        PlayerDisconnectedEvent.Invoke(conn);
    }
    
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        Debug.Log($"Player connected: {conn.connectionId}");
        PlayerConnectedEvent.Invoke(conn);
    }
}