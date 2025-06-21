using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerProfileView : MonoBehaviour
{
    [SerializeField] private RawImage _avatar;
    [SerializeField] private TextMeshProUGUI _nickname;
    
    [Inject] private SteamManager _steamManager;
    
    private async void Start()
    {
        _nickname.text = _steamManager.GetPlayerName();
        _avatar.texture = await _steamManager.GetPlayerAvatarAsync();
    }
}