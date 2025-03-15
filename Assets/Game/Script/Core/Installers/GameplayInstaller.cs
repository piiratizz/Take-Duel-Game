using Mirror;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [Inject] private NetworkManager _networkManager; 
    
    public override void InstallBindings()
    {
        ContainerHolder.AttachContainer(Container);
    }
    
    private T CreateAndBindAsSelf<T>(T prefab) where T : Object
    {
        var instance = Container.InstantiatePrefabForComponent<T>(prefab);
        Container.Bind<T>().FromInstance(instance).AsSingle();
        return instance;
    }
}