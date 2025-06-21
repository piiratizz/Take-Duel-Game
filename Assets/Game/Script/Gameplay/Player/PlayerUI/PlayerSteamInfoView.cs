using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerSteamInfoView : MonoBehaviour
{
    [SerializeField] private RawImage _avatar;
    [SerializeField] private TextMeshProUGUI _nickname;
    
    public void Initialize(string nickname, Texture2D avatar)
    {
        _avatar.texture = avatar;
        _nickname.text = nickname;
    }
}