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
    [Inject] private PlayerLivesService _playerLivesService;
    [Inject] private LoadingScreenService _loadingScreenService;
    [Inject] private GameplayUIRoot _gameplayUI;
    [Inject] private SpawnPointManager _spawnPointManager;
    [Inject] private SceneService _sceneService;
    [Inject] private PlayerStateService _playerStateService;
    [Inject] private PlayersSkinsLoaderService _playersSkinsLoaderService;
    [Inject] private RewardServiceBase _rewardService;
    [Inject] private SteamManager _steamManager;
    [Inject] private NetworkServerStateManager _networkServerStateManager;
    
    private RoundTimerUI _roundTimer;
    private LivesCountUI _playerLivesCounter;

    public void Initialize()
    {
        _roundTimer = _gameplayUI.RoundTimer;
        _playerLivesCounter = _gameplayUI.LivesCounter;

        if(!NetworkServer.active) return;
        InitializeServerSide();
    }

    [Server]
    private async void InitializeServerSide()
    {
        Debug.Log("CHECK IS READY");
        await IsConnectionsReady();
        Debug.Log("CONNECTIONS IS READY");
        await IsPlayersReady();
        Debug.Log("STARTING DUEL");
        InitializePlayersView();
        Debug.Log("INITIALIZED PLAYERS VIEWS");
        InitializePlayersHealths();
        Debug.Log("INITIALIZED PLAYERS HEALTHS");
        StartDuel();
    }

    private async UniTask<bool> IsConnectionsReady()
    {
        await UniTask.WaitUntil(() =>
        {
            foreach (var conn in _networkManager.ConnectedPlayer)
            {
                if (!conn.isReady)
                    return false;
            }

            return true;
        });
        return false;
    }
    
    private async UniTask<bool> IsPlayersReady()
    {
        await UniTask.WaitUntil(() =>
        {
            foreach (var status in _networkServerStateManager.PlayersStatus)
            {
                if (!status.Value)
                {
                    Debug.Log(status);
                    return false;
                }
            }
        
            return true;
        });
        
        return false;
    }

    [Server]
    private async void StartDuel()
    {
        TeleportPlayersToStart();
        
        await StartRoundCountdownAsync();
        _playerStateService.SetAllPlayersState(States.Fight);
    }

    [Server]
    private void InitializePlayersView()
    {
        SetPlayersSteamInfo();
        SetPlayersSkins();
    }

    [Server]
    private void InitializePlayersHealths()
    {
        foreach (var conn in _networkManager.ConnectedPlayer)
        {
            var lives = _playerLivesService.LivesOfPlayer(conn);
            TargetUpdateLivesCount(conn, lives);
        }
    }
    
    [Server]
    private async void OnPlayerDisconnected(NetworkConnectionToClient conn)
    {
        if (!_waitUntilTwoPlayersLoaded) return;
        
        await UniTask.Delay(100);
        _networkManager.StopHost();
    }
    
    private async void OnClientDisconnected()
    {
        await _sceneService.LoadMainMenuAsync();
    }

    [TargetRpc]
    private void TargetUpdateLivesCount(NetworkConnection conn, int lives)
    {
        _playerLivesCounter.UpdateText(lives);
    }

    [Server]
    private void TeleportPlayersToStart()
    {
        var players = _playerLivesService.PlayersConnections;
        var positions = _spawnPointManager.StartPosition;
        for (int i = 0; i < players.Count; i++)
        {
            Debug.Log(players[i].identity);
            Debug.Log(players[i].identity.GetComponent<PlayerRoot>());
            var player = players[i].identity.GetComponent<PlayerRoot>();
            Debug.Log(player);
            player.RpcTeleportTo(positions[i].transform.position);
            player.RpcRotate(positions[i].transform.rotation);
        }
    }

    [Server]
    public async void SendPlayerDead(NetworkConnectionToClient conn, PlayerRoot player)
    {
        _playerStateService.SetAllPlayersState(States.Wait);
        TeleportPlayersToStart();

        foreach (var p in _playerLivesService.PlayersConnections)
        {
            var root = p.identity.GetComponent<PlayerRoot>();
            root.ResetHealthFromServer();
        }

        _playerStateService.SetPlayerState(player.netId, States.Dead);
        _playerLivesService.DecreaseLiveCount(conn);

        int remainingLives = _playerLivesService.LivesOfPlayer(conn);
        TargetUpdateLivesCount(conn, remainingLives);

        if (remainingLives <= 0)
        {
            var winPlayer = _playerLivesService.PlayersConnections.First(c => c != conn);
            Debug.Log($"{winPlayer.identity.netId} WIN the game");
            _rewardService.RewardPlayer(winPlayer);
        }

        await StartRoundCountdownAsync();

        _playerStateService.SetAllPlayersState(States.Fight);
    }

    [ClientRpc]
    private void RpcShowLoadingScreen() => _loadingScreenService.ShowLoadingScreen();

    [ClientRpc]
    private void RpcHideLoadingScreen() => _loadingScreenService.HideLoadingScreen();

    private async UniTask StartRoundCountdownAsync()
    {
        RpcStartTimer();
        await _roundTimer.StartTimerAsync(_timeToStartRound);
    }

    [ClientRpc]
    private void RpcStartTimer()
    {
        _roundTimer.StartTimerAsync(_timeToStartRound).Forget();
    }

    [Server]
    private void SetPlayersSkins()
    {
        _playersSkinsLoaderService.SetSkins();
    }

    [Server]
    private void SetPlayersSteamInfo()
    {
        foreach (var p in _playerLivesService.PlayersConnections)
        {
            var data = _networkManager.GetPlayerData(p);
            var root = p.identity.GetComponent<PlayerRoot>();
            root.RpcInitializePlayerSteamInfo(data.Nickname);
        }
    }
}