using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkServerStateManager : NetworkBehaviour
{
    [SyncVar] private bool _isReady;

    private Dictionary<NetworkConnectionToClient, bool> _playerReadyStatus;
    public IReadOnlyDictionary<NetworkConnectionToClient, bool> PlayersStatus => _playerReadyStatus;
    
    public bool IsReady => _isReady;
    
    [Server]
    public void MarkServerReady()
    {
        _isReady = true;
    }

    public void InitializePlayers(List<NetworkConnectionToClient> connections)
    {
        _playerReadyStatus = new Dictionary<NetworkConnectionToClient, bool>();
        foreach (var conn in connections)
        {
            _playerReadyStatus.Add(conn, false);
        }
    }
    
    [Server]
    public void AddReadyPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log(_playerReadyStatus);
        Debug.Log(_playerReadyStatus[conn]);
        _playerReadyStatus[conn] = true;
    }
}