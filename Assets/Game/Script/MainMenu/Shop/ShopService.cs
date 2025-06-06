using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShopService : MonoBehaviour
{
    [SerializeField] private SkinItemView _skinViewPrefab;
    [SerializeField] private HorizontalLayoutGroup _itemsContainer;
    [SerializeField] private ScrollRect _scrollRect;

    [Inject] private PlayerDataStorageService _playerDataStorageService;
    [Inject] private PlayerBankService _playerBankService;

    private List<SkinItemView> _skinViews;
    private List<SkinData> _skinsData;
    
    public void Start()
    {
        var skins = Resources.LoadAll<SkinData>("Skins");
        _skinViews = new List<SkinItemView>(skins.Length);
        
        var playerData = _playerDataStorageService.Data;
        _skinsData = skins.OrderBy(a => a.price).Except(skins.Where(a => playerData.AvailableSkins.Contains(a.skinName))).ToList();
        
        UpdateView();
    }
    

    public void UpdateView()
    {
        foreach (Transform child in _itemsContainer.transform)
        {
            Destroy(child.gameObject);
        }

        _skinViews = new List<SkinItemView>(_skinsData.Count);

        foreach (var skin in _skinsData)
        {
            var instance = Instantiate(_skinViewPrefab, _itemsContainer.transform);
            instance.AttachData(skin);
            instance.BuyEvent.AddListener(TryBuySkin);
            _skinViews.Add(instance);
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRect.GetComponent<RectTransform>());
    }
    
    private void TryBuySkin(SkinData data)
    {
        Debug.Log("Try to buy");
        if (_playerBankService.TryPay(data.price))
        {
            _playerDataStorageService.AddAvailableSkin(data.skinName);
        }
    }
}
