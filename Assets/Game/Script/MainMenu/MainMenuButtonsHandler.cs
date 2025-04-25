//TEMP Solution
using kcp2k;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuButtonsHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField _ipAddressInputField;
    [SerializeField] private TMP_InputField _portInputField;
    [SerializeField] private Button _connectButton;
    [SerializeField] private Button _hostButton;

    [Inject] private CustomNetworkManager _networkManager;
    
    private void Start()
    {
        _connectButton.onClick = new Button.ButtonClickedEvent();
        _connectButton.onClick.AddListener(Connect);
        
        _hostButton.onClick = new Button.ButtonClickedEvent();
        _hostButton.onClick.AddListener(Host);
    }
    
    
    private void Connect()
    {
        ApplyData();
        _networkManager.StartClient();
    }

    private void Host()
    {
        ApplyData();
        _networkManager.StartHost();
    }

    private void ApplyData()
    {
        if (_ipAddressInputField.text == string.Empty)
        {
            _networkManager.networkAddress = "localhost";
        }
        else
        {
            _networkManager.networkAddress = _ipAddressInputField.text;
        }
        
        var kcp = _networkManager.GetComponent<KcpTransport>();

        if (_portInputField.text == string.Empty)
        {
            kcp.port = 7777;
        }
        else
        {
            kcp.port = ushort.Parse(_portInputField.text);
        }
    }
}
