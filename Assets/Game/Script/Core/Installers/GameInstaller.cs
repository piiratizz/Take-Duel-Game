using Cysharp.Threading.Tasks;
using kcp2k;
using Mirror;
using Mirror.FizzySteam;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private LobbyService _lobbyService;
    [SerializeField] private CustomNetworkManager _networkManager;

    private SteamManager _steamManager;
    
    private LoadingScreenService _loadingScreenService;
    
    public override async void InstallBindings()
    {
        Debug.Log("GAME INSTALLER STARTED");
        Container.Bind<ISaveLoadService>().To<JsonSaveLoadService>().AsTransient().WithArguments("player_data.json");
        
        var playerDataStorageService = new PlayerDataStorageService();
        Container.Bind<PlayerDataStorageService>().FromInstance(playerDataStorageService).AsSingle();
        Container.Inject(playerDataStorageService);
        playerDataStorageService.Initialize();
        
        var networkManagerInstance = Instantiate(_networkManager);
        Container.Bind<CustomNetworkManager>().FromInstance(networkManagerInstance).AsSingle();
        
        _steamManager = networkManagerInstance.GetComponent<SteamManager>();
        _steamManager.Initialize();
        Container.Bind<SteamManager>().FromInstance(_steamManager).AsSingle();
        
        Container.Inject(networkManagerInstance);
        
        var playerBankService = new PlayerBankService();
        Container.Bind<PlayerBankService>().FromInstance(playerBankService).AsSingle();
        Container.Inject(playerBankService);
        playerBankService.Initialize();
        
        _loadingScreenService = new LoadingScreenService();
        Container.Bind<LoadingScreenService>().FromInstance(_loadingScreenService).AsSingle();
        
        SceneService sceneService = new SceneService();
        Container.Bind<SceneService>().FromInstance(sceneService).AsSingle();

        var lobbyServiceInstance = Container.InstantiatePrefabForComponent<LobbyService>(_lobbyService);
        Container.Bind<LobbyService>().FromInstance(lobbyServiceInstance).AsSingle();

        lobbyServiceInstance.Initialize();
        ///
        
        await UniTask.Yield();
        Container.Inject(sceneService);
        _loadingScreenService.Initialize();
        await UniTask.Yield();

        _loadingScreenService.ShowLoadingScreen();
        
        Debug.Log("GAME INSTALLED");
        
        await StartLoadingAsync(sceneService);
        
    }
    
    private async UniTask StartLoadingAsync(SceneService sceneService)
    {
//#if UNITY_EDITOR
        //await sceneService.LoadSceneAsync(SceneManager.GetActiveScene().name);
        //return;
//#endif
        // --- !!!DONT DELETE!!! ---
        await sceneService.LoadMainMenuAsync();
    }

    private T CreateAndBindAsSelf<T>(T prefab) where T : Object
    {
        var instance = Container.InstantiatePrefabForComponent<T>(prefab);
        Container.Bind<T>().FromInstance(instance).AsSingle();
        return instance;
    }
}