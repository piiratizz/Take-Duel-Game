using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventorySkinsService : MonoBehaviour
{
    [SerializeField] private InventorySkinView _skinViewPrefab;
    
    [Inject] private MainMenuUIRoot _menuUIRoot;
    [Inject] private PlayerDataStorageService _playerDataStorageService;

    private PlayerData _playerData;
    private SkinData[] _allSkinsData;
    private SkinData[] _availableSkinsData;

    private InventorySkinView _selectedSkinView;
    
    public void Start()
    {
        _allSkinsData = Resources.LoadAll<SkinData>("Skins");
        UpdateSkinsData();
        UpdateView();
    }

    public void UpdateSkinsData()
    {
        _availableSkinsData = _allSkinsData
            .Where(skin => _playerDataStorageService.Data.AvailableSkins.Contains(skin.skinName))
            .ToArray();
    }
    
    public void UpdateView()
    {
        foreach (Transform child in _menuUIRoot.InventorySkinsItemsContainer.transform)
        {
            Destroy(child.gameObject);
        }
        
        var views = new List<InventorySkinView>(_availableSkinsData.Length);

        foreach (var skin in _availableSkinsData)
        {
            var instance = Instantiate(_skinViewPrefab, _menuUIRoot.InventorySkinsItemsContainer.transform);
            instance.AttachData(skin);
            instance.EquipEvent.AddListener(EquipSkin);
            views.Add(instance);

            if (skin.skinName == _playerDataStorageService.Data.SelectedSkin)
            {
                _selectedSkinView = instance;
            }
            
            instance.OnSkinDeEquip();
        }
        
        _selectedSkinView.OnSkinEquip();
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(_menuUIRoot.InventorySkinsScrollRect.GetComponent<RectTransform>());
    }

    private void EquipSkin(SkinData data, InventorySkinView sender)
    {
        _selectedSkinView.OnSkinDeEquip();
        _selectedSkinView = sender;
        _selectedSkinView.OnSkinEquip();
        
        _playerDataStorageService.SelectSkin(data.skinName);
        Debug.Log($"Selected skin: {data.skinName}");
        
    }
}