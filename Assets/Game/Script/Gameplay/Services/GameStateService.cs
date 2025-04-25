using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using Zenject;

public class GameStateService : NetworkBehaviour
{
    [Inject] private CustomNetworkManager _networkManager;
    [Inject] private ServerPlayersService _serverPlayersService;
    [Inject] private LoadingScreenService _loadingScreenService;
    
    public async void Initialize()
    {
        await UniTask.Yield();
        _networkManager.PlayerSpawnedEvent.AddListener(OnPlayerSpawned);
    }

    [Server]
    private async void OnPlayerSpawned(NetworkConnectionToClient conn)
    {
        await UniTask.Yield();
        await UniTask.Yield();
        await UniTask.WaitForSeconds(2);

        var players = FindObjectsByType<PlayerRoot>(FindObjectsSortMode.None);
        if (players.Length == 2)
        {
            UnfreezePlayers(players);
        }
        else
        {
            FreezePlayers(players);
        }
    }

    
    [Server]
    private void FreezePlayers(PlayerRoot[] players)
    {
        Debug.Log("PLAYER FREEZED");
        foreach (var playerRoot in players)
        {
            playerRoot.SetState(States.Wait);
            RpcFreeze();
        }
    }

    [ClientRpc]
    private void RpcFreeze()
    {
        _loadingScreenService.ShowLoadingScreen();
        Debug.Log("PLAYER FREEZED");
        var players = FindObjectsByType<PlayerRoot>(FindObjectsSortMode.None);
        foreach (var playerRoot in players)
        {
            playerRoot.SetState(States.Wait);
        }
    }

    
    private void UnfreezePlayers(PlayerRoot[] players)
    {
        _loadingScreenService.HideLoadingScreen();
        Debug.Log("PLAYER FREEZED");
        foreach (var playerRoot in players)
        {
            playerRoot.SetState(States.Wait);
            RpcUnfreeze();
        }
    }
    
    [ClientRpc]
    private void RpcUnfreeze()
    {
        Debug.Log("PLAYER UNFREEZED");
        var players = FindObjectsByType<PlayerRoot>(FindObjectsSortMode.None);
        foreach (var playerRoot in players)
        {
            Debug.Log(playerRoot);
            playerRoot.SetState(States.Fight);
        }
    }

    
    [Server]
    public async void SendPlayerDead(NetworkConnectionToClient conn, PlayerRoot player)
    {
        Debug.Log("PLAYER " + player + " DEAD");

        if (!isServer)
        {
            player.SetState(States.Dead);
        }

        RpcOnPlayerDead(player);

        _serverPlayersService.DecreaseLiveCount(conn);
        if (_serverPlayersService.LivesOfPlayer(conn) <= 0)
        {
            Debug.Log(player.netId + " Lose the game");
        }

        await UniTask.WaitForSeconds(3);

        player.SetState(States.Fight);
        RpcAfterPlayerDead(player);

        Debug.Log(_serverPlayersService.LivesOfPlayer(conn));
    }

    [ClientRpc]
    private void RpcAfterPlayerDead(PlayerRoot player)
    {
        player.SetState(States.Fight);
    }

    [ClientRpc]
    private void RpcOnPlayerDead(PlayerRoot player)
    {
        player.SetState(States.Dead);
        TeleportPlayersToStartPosition();
    }
    

    private void TeleportPlayersToStartPosition()
    {
        var players = FindObjectsByType<PlayerRoot>(FindObjectsSortMode.None);
        var points = FindObjectsByType<NetworkStartPosition>(FindObjectsSortMode.None);

        players[0].transform.position = points[0].transform.position;
        players[1].transform.position = points[1].transform.position;
    }
}