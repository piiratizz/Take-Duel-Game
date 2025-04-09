using Zenject;
using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    [Header("CUSTOM SECTION")] 
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private NetworkStartPosition[] _spawnPoints;
    
    [Inject] private DiContainer _container;

    private GameObject _lastSpawnedPlayer;
    
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
        Debug.Log("PLAYER SPAWNED");
    }
}