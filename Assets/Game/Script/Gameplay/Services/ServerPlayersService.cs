using System.Collections.Generic;
using System.Linq;
using Mirror;
using R3;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class ServerPlayersService : NetworkBehaviour
{
    [SerializeField] private int _livesCountForEachPlayer;
    [Inject] private CustomNetworkManager _networkManager;
    
    private Dictionary<NetworkConnectionToClient, int> _playersLives;
    
    public List<NetworkConnectionToClient> PlayersConnections => _playersLives.Keys.ToList();
    [HideInInspector] public UnityEvent<int> OnPlayerCountChanged;
    public int PlayerCount => _playersLives.Count;
    
    public void Initialize()
    {
        OnPlayerCountChanged = new UnityEvent<int>();
        _playersLives = new Dictionary<NetworkConnectionToClient, int>();
        _networkManager.PlayerConnectedEvent.AddListener(RegisterNewPlayer);
        _networkManager.PlayerDisconnectedEvent.AddListener(DeletePlayer);
    }
    
    [Server]
    private void RegisterNewPlayer(NetworkConnectionToClient player)
    {
        Debug.Log("Registered new player " + player.connectionId);
        _playersLives.Add(player, _livesCountForEachPlayer);
        OnPlayerCountChanged.Invoke(PlayerCount);
    }
    
    [Server]
    private void DeletePlayer(NetworkConnectionToClient player)
    {
        _playersLives.Remove(player);
        OnPlayerCountChanged.Invoke(PlayerCount);
    }

    [Server]
    public void DecreaseLiveCount(NetworkConnectionToClient player)
    {
        _playersLives[player]--;
    }

    [Server]
    public void ResetLiveCount(NetworkConnectionToClient player)
    {
        _playersLives[player] = _livesCountForEachPlayer;
    }
    
    [Server]
    public int LivesOfPlayer(NetworkConnectionToClient player) => _playersLives[player];

    [Server]
    public NetworkIdentity GetPlayerById(uint id)
    {
        var players = _playersLives.Keys;
        var require = players.First(p => p.identity.netId == id);
        return require.identity;
    }
}