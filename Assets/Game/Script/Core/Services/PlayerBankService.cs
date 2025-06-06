using R3;
using UnityEngine;
using Zenject;

public class PlayerBankService
{
    [Inject] private PlayerDataStorageService _playerDataStorageService;
    
    private ReactiveProperty<decimal> _balanceReactive;
    public Observable<decimal> Balance => _balanceReactive;

    private PlayerData _playerDataReadOnly;
    
    public void Initialize()
    {
        _balanceReactive = new ReactiveProperty<decimal>();
        _playerDataReadOnly = _playerDataStorageService.Data;
        _balanceReactive.Value = _playerDataReadOnly.Balance;

        _balanceReactive.Subscribe(_playerDataStorageService.UpdateBalance);
    }
    
    public void Add(decimal value)
    {
        _balanceReactive.Value += value;
    }

    public bool TryPay(decimal value)
    {
        if (_balanceReactive.Value - value < 0)
        {
            return false;
        }
        
        _balanceReactive.Value -= value;
        return true;
    }
}