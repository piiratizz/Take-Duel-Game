using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [SerializeField] private LobbyService _lobbyService;
    [Inject] private LoadingScreenService _loadingScreenService;
    
    public override void InstallBindings()
    {
        Debug.Log("MAIN MENU INSTALLER STARTED");
        _loadingScreenService.HideLoadingScreen();
        Cursor.lockState = CursorLockMode.Confined;

        Container.Bind<LobbyService>().FromInstance(_lobbyService).AsSingle();
        Container.Inject(_lobbyService);
        
        _lobbyService.Initialize();
    }
}