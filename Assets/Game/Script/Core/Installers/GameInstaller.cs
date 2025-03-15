using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private UIRoot _uiRoot;
    [SerializeField] private NetworkManager _networkManager;
    
    public override async void InstallBindings()
    {
        CreateAndBindAsSelf(_uiRoot);
        CreateAndBindAsSelf(_networkManager);
        
        Container.Bind<LoadingScreenService>().FromNew().AsSingle();
        
        SceneService sceneService = new SceneService();
        Container.Inject(sceneService);
        Container.Bind<SceneService>().FromInstance(sceneService).AsSingle();
        
        await UniTask.Yield(PlayerLoopTiming.Update);
        var loadingScreenService = Container.Resolve<LoadingScreenService>();
        loadingScreenService.Initialize();
        
        await StartLoadingAsync(sceneService);
    }
    
    private async UniTask StartLoadingAsync(SceneService sceneService)
    {
#if UNITY_EDITOR
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        await sceneService.LoadSceneAsync(scene.name);
        return;
#endif
        //---DONT DELETE---
        await sceneService.LoadBootAsync();
        await sceneService.LoadMainMenuAsync();
    }

    private T CreateAndBindAsSelf<T>(T prefab) where T : Object
    {
        var instance = Container.InstantiatePrefabForComponent<T>(prefab);
        Container.Bind<T>().FromInstance(instance).AsSingle();
        return instance;
    }
}