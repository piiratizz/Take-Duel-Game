using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuUIRoot : MonoBehaviour
{
    public TextMeshProUGUI LobbyText;
    public HorizontalLayoutGroup ShopItemsContainer;
    public ScrollRect ShopItemsScrollRect;
    
    public HorizontalLayoutGroup InventorySkinsItemsContainer;
    public ScrollRect InventorySkinsScrollRect;

    public Button PlayBtn;
    public Button ShopBtn;
    public Button SkinsBtn;
    public Button QuitBtn;
    public Button MainMenuBtn;

    public MainMenuWindow MainMenuWindow;
    public PlayWindow PlayWindow;
    public ShopWindow ShopWindow;
    public InventorySkinsWindow InventorySkinsWindow;
    public LobbyListWindow LobbyListWindow;
    public LobbyCreationWindow LobbyCreationWindow;

    public TMP_InputField PortInputField;
    public TMP_InputField IpInputField;
    
    public Button ConnectButton;
    public Button HostButton;
    public Button CreateLobbyButton;

    public LobbyItemsManager LobbyItemsManager;
    public LobbyCreator LobbyCreator;
    public WindowsManager WindowsManager;
}