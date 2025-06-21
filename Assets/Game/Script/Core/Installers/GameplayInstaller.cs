using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private GameplayUIRoot _gameplayUIPrefab;
    [SerializeField] private ServerPlayersService _serverPlayersService;
    [SerializeField] private GameStateService _gameStateService;
    [SerializeField] private SpawnPointManager _spawnPointManager;
    [SerializeField] private PlayerStateService _playerStateService;
    [FormerlySerializedAs("_rewardService")] [SerializeField] private BalanceRewardService rewardServiceService;
    
    public override void InstallBindings()
    {
        ContainerHolder.AttachContainer(Container);
        CreateAndBindAsSelf(_gameplayUIPrefab);

        Container.Bind<ServerPlayersService>().FromInstance(_serverPlayersService).AsSingle().NonLazy();
        Container.Inject(_serverPlayersService);
        _serverPlayersService.Initialize();
        
        Container.Bind<SpawnPointManager>().FromInstance(_spawnPointManager).AsSingle();

        Container.Bind<PlayerStateService>().FromInstance(_playerStateService).AsSingle();
        
        Container.Bind<PlayersSkinsLoaderService>().FromNew().AsSingle();
        
        Container.Bind<RewardServiceBase>().FromInstance(rewardServiceService).AsSingle();
        Container.Inject(rewardServiceService);
        
        Container.Bind<GameStateService>().FromInstance(_gameStateService).AsSingle().NonLazy();
        Container.Inject(_gameStateService);
        _gameStateService.Initialize();
        
        Debug.Log("GAMEPLAY INITIALIZED");
    }
    
    private T CreateAndBindAsSelf<T>(T prefab) where T : Object
    {
        var instance = Container.InstantiatePrefabForComponent<T>(prefab);
        Container.Bind<T>().FromInstance(instance).AsSingle();
        return instance;
    }
}