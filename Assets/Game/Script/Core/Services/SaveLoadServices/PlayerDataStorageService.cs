using UnityEngine;
using Zenject;

public class PlayerDataStorageService
{
    [Inject] private ISaveLoadService _saveLoadService;

    private PlayerData _playerData;
    public PlayerData Data => _playerData.Clone() as PlayerData;

    public void Initialize()
    {
        Load();
    }
    
    public void UpdateBalance(decimal balance)
    {
        _playerData.Balance = balance;
        Save();
    }

    public void SelectSkin(string skinName)
    {
        _playerData.SelectedSkin = skinName;
        Save();
    }

    public void AddAvailableSkin(string skinName)
    {
        _playerData.AvailableSkins.Add(skinName);
        Save();
    }
    
    public void Load()
    {
        _playerData = _saveLoadService.Load();
    }

    public void Save()
    {
        _saveLoadService.Save(_playerData);
    }
}