using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private CustomNetworkManager _networkManager;

    private LoadingScreenService _loadingScreenService;
    
    public override async void InstallBindings()
    {
        Debug.Log("GAME INSTALLER STARTED");
        CreateAndBindAsSelf(_networkManager);

        _loadingScreenService = new LoadingScreenService();
        Container.Bind<LoadingScreenService>().FromInstance(_loadingScreenService).AsSingle();
        
        SceneService sceneService = new SceneService();
        Container.Bind<SceneService>().FromInstance(sceneService).AsSingle();
        
        await UniTask.Yield();
        Container.Inject(sceneService);
        _loadingScreenService.Initialize();
        await UniTask.Yield();

        _loadingScreenService.ShowLoadingScreen();
        await StartLoadingAsync(sceneService);
    }
    
    private async UniTask StartLoadingAsync(SceneService sceneService)
    {
#if UNITY_EDITOR
        await sceneService.LoadSceneAsync(SceneManager.GetActiveScene().name);
        return;
#endif
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