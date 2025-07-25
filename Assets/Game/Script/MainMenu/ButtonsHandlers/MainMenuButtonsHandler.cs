using kcp2k;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuButtonsHandler : MonoBehaviour
{
    [Inject] private LobbyService _lobbyService;
    [Inject] private MainMenuUIRoot _mainMenuUIRoot;
    [Inject] private CustomNetworkManager _networkManager;
    [Inject] private SceneService _sceneService;
    
    private WindowsManager _windowsManager;
    
    private void Start()
    {
        _mainMenuUIRoot.ConnectButton.onClick = new Button.ButtonClickedEvent();
        _mainMenuUIRoot.ConnectButton.onClick.AddListener(Connect);
        
        _mainMenuUIRoot.HostButton.onClick = new Button.ButtonClickedEvent();
        _mainMenuUIRoot.HostButton.onClick.AddListener(Host);
        
        _mainMenuUIRoot.CreateLobbyButton.onClick = new Button.ButtonClickedEvent();
        _mainMenuUIRoot.CreateLobbyButton.onClick.AddListener(CreateLobby);

        _windowsManager = _mainMenuUIRoot.WindowsManager;
    }

    private void CreateLobby()
    {
        _mainMenuUIRoot.LobbyCreator.Create();
    }


    private async void Connect()
    {
        ApplyData();
        await _sceneService.LoadGameplayAsync();
        _networkManager.StartClient();
    }

    private async void Host()
    {
        ApplyData();
        await _sceneService.LoadGameplayAsync();
        _networkManager.StartHost();
    }

    private void ApplyData()
    {
        if (_mainMenuUIRoot.IpInputField.text == string.Empty)
        {
            _networkManager.networkAddress = "localhost";
        }
        else
        {
            _networkManager.networkAddress = _mainMenuUIRoot.IpInputField.text;
        }
        
        var kcp = _networkManager.GetComponent<KcpTransport>();

        if (_mainMenuUIRoot.PortInputField.text == string.Empty)
        {
            kcp.port = 7777;
        }
        else
        {
            kcp.port = ushort.Parse(_mainMenuUIRoot.PortInputField.text);
        }
        Debug.Log($"{_networkManager.networkAddress} {kcp.port}");
    }
}
