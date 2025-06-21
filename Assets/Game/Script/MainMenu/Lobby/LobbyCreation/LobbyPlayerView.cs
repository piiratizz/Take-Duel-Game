using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerView : MonoBehaviour
{
    [SerializeField] private RawImage _avatarImage;
    [SerializeField] private TextMeshProUGUI _nicknameText;

    public void Initialize(Texture2D avatar, string nickname)
    {
        _avatarImage.texture = avatar;
        _nicknameText.text = nickname;
    }
}