using Mirror;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [Inject] private NetworkManager _networkManager;
    [Inject] private UIRoot _uiRoot;
    [SerializeField] private GameplayUIRoot _gameplayUIPrefab;
    
    public override void InstallBindings()
    {
        ContainerHolder.AttachContainer(Container);
        var instance = CreateAndBindAsSelf(_gameplayUIPrefab);
        _uiRoot.AttachUI(instance.gameObject);
    }
    
    private T CreateAndBindAsSelf<T>(T prefab) where T : Object
    {
        var instance = Container.InstantiatePrefabForComponent<T>(prefab);
        Container.Bind<T>().FromInstance(instance).AsSingle();
        return instance;
    }
}