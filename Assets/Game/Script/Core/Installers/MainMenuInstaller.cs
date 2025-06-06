using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [SerializeField] private LobbyService _lobbyService;
    [SerializeField] private ShopService _shopService;
    [SerializeField] private InventorySkinsService _inventorySkinsService;
    [Inject] private LoadingScreenService _loadingScreenService;
    
    public override async void InstallBindings()
    {
        Debug.Log("MAIN MENU INSTALLER STARTED");
        await UniTask.Yield();
        _loadingScreenService.HideLoadingScreen();
        Cursor.lockState = CursorLockMode.None;
        
        Container.Bind<LobbyService>().FromInstance(_lobbyService).AsSingle();
        
        Container.Bind<ShopService>().FromInstance(_shopService).AsSingle();
        Container.Bind<InventorySkinsService>().FromInstance(_inventorySkinsService).AsSingle();
        
        Container.Inject(_lobbyService);
        
        _lobbyService.Initialize();
    }
}