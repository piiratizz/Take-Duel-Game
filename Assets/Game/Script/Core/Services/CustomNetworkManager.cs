using Zenject;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class CustomNetworkManager : NetworkManager
{
    [Header("CUSTOM SECTION")] 
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private NetworkStartPosition[] _spawnPoints;
    
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
        _spawnPoints = FindObjectsByType<NetworkStartPosition>(FindObjectsSortMode.None);
        
        NetworkStartPosition startPos = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        
        GameObject player = Instantiate(
            _playerPrefab,
            startPos.transform.position,
            startPos.transform.rotation,
            null);
        _lastSpawnedPlayer = player;
        
        NetworkServer.AddPlayerForConnection(conn, player);
        PlayerSpawnedEvent.Invoke(conn);
        Debug.Log("PLAYER SPAWNED");
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.Log($"Player disconnected: {conn.connectionId}");
        PlayerDisconnectedEvent.Invoke(conn);
    }
    
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        Debug.Log($"Player connected: {conn.connectionId}");
        PlayerConnectedEvent.Invoke(conn);
    }
}