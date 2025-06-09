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
    [SerializeField] private bool _useSteamConnection;
    
    private KcpTransport _mirrorKcp;
    private FizzySteamworks _steamKcp;
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
        
        var networkManagerInstance = CreateAndBindAsSelf(_networkManager);

        _mirrorKcp = networkManagerInstance.GetComponent<KcpTransport>();
        _steamKcp = networkManagerInstance.GetComponent<FizzySteamworks>();
        _steamManager = networkManagerInstance.GetComponent<SteamManager>();
        _steamManager.Initialize();
        
        var playerBankService = new PlayerBankService();
        Container.Bind<PlayerBankService>().FromInstance(playerBankService).AsSingle();
        Container.Inject(playerBankService);
        playerBankService.Initialize();
        
        if (_useSteamConnection)
        {
            networkManagerInstance.transport = _steamKcp;
            _mirrorKcp.enabled = false;
        }
        else
        {
            networkManagerInstance.transport = _mirrorKcp;
            _steamKcp.enabled = false;
        }
        
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