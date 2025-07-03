using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerSteamInfoView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nickname;
    
    public void Initialize(string nickname)
    {
        _nickname.text = nickname;
    }
}