using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [SerializeField] private ShopService _shopService;
    [SerializeField] private InventorySkinsService _inventorySkinsService;
    [SerializeField] private MainMenuUIRoot _mainMenuUI;
    [SerializeField] private WindowsManager _windowsManager;
    [SerializeField] private MainMenuButtonsHandler _mainMenuButtonsHandler;
    [Inject] private LoadingScreenService _loadingScreenService;
    
    public override async void InstallBindings()
    {
        Debug.Log("MAIN MENU INSTALLER STARTED");
        await UniTask.Yield();
        _loadingScreenService.HideLoadingScreen();
        Cursor.lockState = CursorLockMode.None;
        
        var shopInstance = Instantiate(_shopService).GetComponent<ShopService>();
        Container.Bind<ShopService>().FromInstance(shopInstance).AsSingle();

        var inventorySkinsInstance = Instantiate(_inventorySkinsService).GetComponent<InventorySkinsService>();
        Container.Bind<InventorySkinsService>().FromInstance(inventorySkinsInstance).AsSingle();

        var mainMenuUIRootInstance = Container.InstantiatePrefabForComponent<MainMenuUIRoot>(_mainMenuUI);
        Container.Bind<MainMenuUIRoot>().FromInstance(mainMenuUIRootInstance).AsSingle();
        
        var windowsManagerInstance = Container.InstantiatePrefabForComponent<WindowsManager>(_windowsManager);
        Container.Bind<WindowsManager>().FromInstance(windowsManagerInstance).AsSingle();
        
        var mainMenuButtonsHandlerInstance = Container.InstantiatePrefabForComponent<MainMenuButtonsHandler>(_mainMenuButtonsHandler);
        
        Container.Inject(shopInstance);
        Container.Inject(inventorySkinsInstance);
        Container.Inject(mainMenuUIRootInstance);
        Container.Inject(mainMenuButtonsHandlerInstance);
        
        Debug.Log("MAIN MENU INSTALLED");
    }
}