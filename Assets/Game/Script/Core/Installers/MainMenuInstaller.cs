using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [SerializeField] private LobbyService _lobbyService;
    [Inject] private LoadingScreenService _loadingScreenService;
    
    public override async void InstallBindings()
    {
        Debug.Log("MAIN MENU INSTALLER STARTED");
        await UniTask.Yield();
        _loadingScreenService.HideLoadingScreen();
        Cursor.lockState = CursorLockMode.Confined;
        
        Container.Bind<LobbyService>().FromInstance(_lobbyService).AsSingle();
        Container.Inject(_lobbyService);
        
        _lobbyService.Initialize();
    }
}