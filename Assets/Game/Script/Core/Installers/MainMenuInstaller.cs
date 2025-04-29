using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [Inject] private LoadingScreenService _loadingScreenService;
    
    public override void InstallBindings()
    {
        _loadingScreenService.HideLoadingScreen();
        Cursor.lockState = CursorLockMode.Confined;
    }
}