using System.Linq;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using Zenject;

public class GameStateService : NetworkBehaviour
{
    [SerializeField] private bool _waitUntilTwoPlayersLoaded = true;
    [SerializeField] private float _timeToStartRound = 5;
    
    [Inject] private CustomNetworkManager _networkManager;
    [Inject] private ServerPlayersService _serverPlayersService;
    [Inject] private LoadingScreenService _loadingScreenService;
    [Inject] private GameplayUIRoot _gameplayUI;
    [Inject] private SpawnPointManager _spawnPointManager;
    
    private RoundTimerUI _roundTimer;
    private LivesCountUI _playerLivesCounter;
    
    public async void Initialize()
    {
        await UniTask.Yield();
        _networkManager.PlayerSpawnedEvent.AddListener(OnPlayerSpawned);
        _roundTimer = _gameplayUI.RoundTimer;
        _playerLivesCounter = _gameplayUI.LivesCounter;
    }

    [Server]
    private async void OnPlayerSpawned(NetworkConnectionToClient conn)
    {
        if(!_waitUntilTwoPlayersLoaded) return;

        RpcShowLoadingScreen();
        await UniTask.WaitForSeconds(0.5f);

        var players = FindObjectsByType<PlayerRoot>(FindObjectsSortMode.None);
        
        FreezePlayers(players);

        var lives = _serverPlayersService.LivesOfPlayer(conn);
        UpdateLivesCountOnPlayer(conn, lives);
        
        if (players.Length != 2) return;
        TeleportPlayersToStartPosition();
        RpcHideLoadingScreen();
        
        RpcStartTimer();
        await _roundTimer.StartTimerAsync(_timeToStartRound);
        
        UnfreezePlayers(players);
        
    }
    
    [TargetRpc]
    private void UpdateLivesCountOnPlayer(NetworkConnection connectionOfPlayer, int lives)
    {
        _playerLivesCounter.UpdateText(lives);
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
        Debug.Log("PLAYER FREEZED");
        var players = FindObjectsByType<PlayerRoot>(FindObjectsSortMode.None);
        foreach (var playerRoot in players)
        {
            playerRoot.SetState(States.Wait);
        }
    }

    
    private void UnfreezePlayers(PlayerRoot[] players)
    {
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

        var players = FindObjectsByType<PlayerRoot>(FindObjectsSortMode.None);
        FreezePlayers(players);
        TeleportPlayersToStartPosition();
        RpcOnPlayerDead(player);

        _serverPlayersService.DecreaseLiveCount(conn);
        
        var lives = _serverPlayersService.LivesOfPlayer(conn);
        UpdateLivesCountOnPlayer(conn, lives);
        
        
        if (_serverPlayersService.LivesOfPlayer(conn) <= 0)
        {
            Debug.Log(player.netId + " Lose the game");
        }
        
        RpcStartTimer();
        await _roundTimer.StartTimerAsync(_timeToStartRound);
        
        player.SetState(States.Fight);
        UnfreezePlayers(players);
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
    }

    [ClientRpc]
    private void RpcStartTimer()
    {
        _roundTimer.StartTimerAsync(_timeToStartRound);
    }
    
    [Server]
    private void TeleportPlayersToStartPosition()
    {
        var players = _serverPlayersService.PlayersConnections;

        var startPositions = _spawnPointManager.StartPosition;

        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i].identity.GetComponent<PlayerRoot>();
            player.StopMove();
            player.TeleportTo(startPositions[i].transform.position);
        }
    }

    [ClientRpc]
    private void RpcShowLoadingScreen()
    {
        _loadingScreenService.ShowLoadingScreen();
    }
    
    [ClientRpc]
    private void RpcHideLoadingScreen()
    {
        _loadingScreenService.HideLoadingScreen();
    }
}