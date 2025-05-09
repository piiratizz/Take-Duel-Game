using System;
using R3;
using TMPro;
using UnityEngine;
using Zenject;

public class PlayerBalanceView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _balanceText;
    [Inject] private PlayerBankService _playerBankService;

    private IDisposable _subscription;
    
    private void Start()
    {
        _subscription = _playerBankService.Balance.Subscribe(ctx => OnBalanceUpdated(ctx));
    }

    private void OnBalanceUpdated(decimal balance)
    {
        _balanceText.text = balance.ToString();
    }

    private void OnDestroy()
    {
        _subscription.Dispose();
    }
}