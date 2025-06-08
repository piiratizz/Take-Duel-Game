using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShopService : MonoBehaviour
{
    [SerializeField] private SkinItemView _skinViewPrefab;

    [Inject] private MainMenuUIRoot _menuUIRoot;
    [Inject] private PlayerDataStorageService _playerDataStorageService;
    [Inject] private PlayerBankService _playerBankService;

    private List<SkinItemView> _skinViews;
    private List<SkinData> _skinsData;
    private SkinData[] _allSkins;
    
    public void Start()
    {
        _allSkins = Resources.LoadAll<SkinData>("Skins");
        _skinViews = new List<SkinItemView>(_allSkins.Length);
        
        UpdateSkinsData();
        UpdateView();
    }
    

    public void UpdateView()
    {
        foreach (Transform child in _menuUIRoot.ShopItemsContainer.transform)
        {
            Destroy(child.gameObject);
        }

        _skinViews = new List<SkinItemView>(_skinsData.Count);

        foreach (var skin in _skinsData)
        {
            var instance = Instantiate(_skinViewPrefab, _menuUIRoot.ShopItemsContainer.transform);
            instance.AttachData(skin);
            instance.BuyEvent.AddListener(TryBuySkin);
            _skinViews.Add(instance);
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(_menuUIRoot.ShopItemsScrollRect.GetComponent<RectTransform>());
    }

    private void UpdateSkinsData()
    {
        var playerData = _playerDataStorageService.Data;
        _skinsData = _allSkins.OrderBy(a => a.price)
            .Except(
                _allSkins.Where(a => 
                    playerData.AvailableSkins.Contains(a.skinName)
                    )
                ).ToList();
    }
    
    private void TryBuySkin(SkinData data)
    {
        Debug.Log("Try to buy");
        if (_playerBankService.TryPay(data.price))
        {
            _playerDataStorageService.AddAvailableSkin(data.skinName);
            UpdateSkinsData();
            UpdateView();
        }
    }
}
