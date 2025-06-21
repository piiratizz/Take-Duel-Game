using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nicknameText;
    [SerializeField] private Button _joinButton;
    
    [HideInInspector] public UnityEvent<LobbyData> JoinClicked;
    private LobbyData _lobbyData;
    
    public void Initialize(LobbyData lobbyData)
    {
        _lobbyData = lobbyData;
        _nicknameText.text = lobbyData.RoomName;
        JoinClicked = new UnityEvent<LobbyData>();
    }
}
