using System.Linq;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private GameplayUIRoot _gameplayUIPrefab;
    [FormerlySerializedAs("_serverPlayersService")] [SerializeField] private PlayerLivesService playerLivesService;
    [SerializeField] private GameStateService _gameStateService;
    [SerializeField] private SpawnPointManager _spawnPointManager;
    [SerializeField] private PlayerStateService _playerStateService;
    [SerializeField] private NetworkServerStateManager _serverStateManager;
    [Inject] private CustomNetworkManager _customNetworkManager;
    [FormerlySerializedAs("_rewardService")] [SerializeField] private BalanceRewardService rewardServiceService;
    
    public override async void InstallBindings()
    {
        ContainerHolder.AttachContainer(Container);

        if (NetworkClient.active && NetworkServer.active)
        {
            _serverStateManager.InitializePlayers(NetworkServer.connections.Values.ToList());
        }
        
        // IF CLIENT
        if (NetworkClient.active && !NetworkServer.active)
        {
            await UniTask.WaitWhile(() => _serverStateManager.IsReady == false);
        }
        
        CreateAndBindAsSelf(_gameplayUIPrefab);

        Container.Bind<SpawnPointManager>().FromInstance(_spawnPointManager).AsSingle();
        Container.Bind<PlayersSkinsLoaderService>().FromNew().AsSingle();
        Container.Bind<NetworkServerStateManager>().FromInstance(_serverStateManager).AsSingle();

        if (NetworkServer.active)
        {
            var serverPlayersServiceInstance = Instantiate(playerLivesService);
            var playerStateServiceInstance = Instantiate(_playerStateService);
            
            var rewardServiceServiceInstance = Instantiate(rewardServiceService);
            var gameStateServiceInstance = Instantiate(_gameStateService);
            
            NetworkServer.Spawn(serverPlayersServiceInstance.gameObject);
            NetworkServer.Spawn(playerStateServiceInstance.gameObject);
            NetworkServer.Spawn(rewardServiceServiceInstance.gameObject);
            NetworkServer.Spawn(gameStateServiceInstance.gameObject);
        }
        
        PlayerLivesService playerLivesServiceInstance1 = FindFirstObjectByType<PlayerLivesService>();
        Container.Bind<PlayerLivesService>().FromInstance(playerLivesServiceInstance1).AsSingle().NonLazy();
        Container.Inject(playerLivesServiceInstance1);
        playerLivesServiceInstance1.Initialize();

        PlayerStateService playerStateServiceInstance1 = FindFirstObjectByType<PlayerStateService>();
        Container.Bind<PlayerStateService>().FromInstance(playerStateServiceInstance1).AsSingle();
        Container.Inject(playerStateServiceInstance1);
        
        RewardServiceBase rewardServiceServiceInstance1 = FindFirstObjectByType<RewardServiceBase>();
        Container.Bind<RewardServiceBase>().FromInstance(rewardServiceServiceInstance1).AsSingle();
        Container.Inject(rewardServiceServiceInstance1);

        GameStateService gameStateServiceInstance1 = FindFirstObjectByType<GameStateService>();
        Container.Bind<GameStateService>().FromInstance(gameStateServiceInstance1).AsSingle().NonLazy();
        Container.Inject(gameStateServiceInstance1);
        gameStateServiceInstance1.Initialize();
        
        Debug.Log("GAMEPLAY INITIALIZED");
        
        await UniTask.WaitWhile(() => !NetworkClient.ready);
        
        // IF HOST
        if (NetworkServer.active && NetworkClient.active)
        {
            Debug.Log("Server is ready");
            _serverStateManager.MarkServerReady();
        }
    }
    
    private T CreateAndBindAsSelf<T>(T prefab) where T : Object
    {
        var instance = Container.InstantiatePrefabForComponent<T>(prefab);
        Container.Bind<T>().FromInstance(instance).AsSingle();
        return instance;
    }
}