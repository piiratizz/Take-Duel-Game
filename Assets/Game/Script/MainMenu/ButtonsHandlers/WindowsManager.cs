using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WindowsManager : MonoBehaviour
{
    [SerializeField] private MainMenuUIRoot _menuUIRoot;

    private WindowBase _currentWindow;
    
    private void Start()
    {
        _menuUIRoot.MainMenuBtn.onClick = new Button.ButtonClickedEvent();
        _menuUIRoot.MainMenuBtn.onClick.AddListener(BackToMainMenu);
        
        _menuUIRoot.PlayBtn.onClick = new Button.ButtonClickedEvent();
        _menuUIRoot.PlayBtn.onClick.AddListener(ShowPlay);
        
        _menuUIRoot.ShopBtn.onClick = new Button.ButtonClickedEvent();
        _menuUIRoot.ShopBtn.onClick.AddListener(ShowShop);
        
        _menuUIRoot.SkinsBtn.onClick = new Button.ButtonClickedEvent();
        _menuUIRoot.SkinsBtn.onClick.AddListener(ShowSkins);
        
        _menuUIRoot.QuitBtn.onClick = new Button.ButtonClickedEvent();
        _menuUIRoot.QuitBtn.onClick.AddListener(Quit);
        
        ShowWindow(_menuUIRoot.MainMenuWindow);
    }

    private void BackToMainMenu()
    {
        ShowWindow(_menuUIRoot.MainMenuWindow);
    }
    
    private void ShowPlay()
    {
        ShowWindow(_menuUIRoot.LobbyListWindow);
        //ShowWindow(_menuUIRoot.PlayWindow);
    }
    
    private void ShowShop()
    {
        ShowWindow(_menuUIRoot.ShopWindow);
    }
    
    private void ShowSkins()
    {
        ShowWindow(_menuUIRoot.InventorySkinsWindow);
    }

    private void Quit()
    {
        Application.Quit();
    }

    public void ShowLobby()
    {
        ShowWindow(_menuUIRoot.LobbyCreationWindow);
    }
    
    private void ShowWindow(WindowBase window)
    {
        if (_currentWindow != null)
        {
            _currentWindow.Close();
        }
        
        _currentWindow = window;
        _currentWindow.Open();
    }
}
