using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    [Header("CUSTOM SECTION")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private NetworkStartPosition[] _spawnPoints;
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        _spawnPoints = FindObjectsByType<NetworkStartPosition>(FindObjectsSortMode.None);
        NetworkStartPosition startPos = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        GameObject player = Instantiate(_playerPrefab, startPos.transform.position, startPos.transform.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);
        Debug.Log("PLAYER SPAWNED");
    }
}