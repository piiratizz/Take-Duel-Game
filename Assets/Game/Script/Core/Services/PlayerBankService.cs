using R3;
using UnityEngine;
using Zenject;

public class PlayerBankService
{
    [Inject] private ISaveLoadService<DecimalWrapper> _saveLoadService; 
    
    private ReactiveProperty<decimal> _balanceReactive;
    public Observable<decimal> Balance => _balanceReactive;

    public void Initialize()
    {
        _balanceReactive = new ReactiveProperty<decimal>();
        Load();
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

    public void Load()
    {
        _balanceReactive.Value = _saveLoadService.Load().value;
    }
    
    public void Save()
    {
        _saveLoadService.Save(new DecimalWrapper(_balanceReactive.Value));
    }
}