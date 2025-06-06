using UnityEngine;
using UnityEngine.UI;

public class WindowsManager : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _skinsButton;
    [SerializeField] private Button _quitButton;

    [SerializeField] private GameObject _mainMenuWindow;
    [SerializeField] private GameObject _playWindow;
    [SerializeField] private GameObject _settingsWindow;
    [SerializeField] private GameObject _shopWindow;
    [SerializeField] private GameObject _skinsWindow;

    private GameObject _currentWindow;
    
    private void Start()
    {
        _mainMenuButton.onClick = new Button.ButtonClickedEvent();
        _mainMenuButton.onClick.AddListener(BackToMainMenu);
        
        _playButton.onClick = new Button.ButtonClickedEvent();
        _playButton.onClick.AddListener(ShowPlay);
        
        _settingsButton.onClick = new Button.ButtonClickedEvent();
        _settingsButton.onClick.AddListener(ShowSettings);
        
        _shopButton.onClick = new Button.ButtonClickedEvent();
        _shopButton.onClick.AddListener(ShowShop);
        
        _skinsButton.onClick = new Button.ButtonClickedEvent();
        _skinsButton.onClick.AddListener(ShowSkins);
        
        _quitButton.onClick = new Button.ButtonClickedEvent();
        _quitButton.onClick.AddListener(Quit);
        
    }

    private void BackToMainMenu()
    {
        ShowWindow(_mainMenuWindow);
    }
    
    private void ShowPlay()
    {
        ShowWindow(_playWindow);
    }

    private void ShowSettings()
    {
        ShowWindow(_settingsWindow);
    }

    private void ShowShop()
    {
        ShowWindow(_shopWindow);
    }
    
    private void ShowSkins()
    {
        ShowWindow(_skinsWindow);
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void ShowWindow(GameObject window)
    {
        if (_currentWindow != null)
        {
            _currentWindow.SetActive(false);
        }
        
        _currentWindow = window;
        _currentWindow.SetActive(true);
    }
}
