using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventorySkinsService : MonoBehaviour
{
    [SerializeField] private InventorySkinView _skinViewPrefab;
    [SerializeField] private HorizontalLayoutGroup _itemsContainer;
    [SerializeField] private ScrollRect _scrollRect;
    
    [Inject] private PlayerDataStorageService _playerDataStorageService;

    private PlayerData _playerData;
    private SkinData[] _skinsData;
    
    public void Start()
    {
        _skinsData = Resources.LoadAll<SkinData>("Skins");
        _skinsData = _skinsData
            .Where(skin => _playerDataStorageService.Data.AvailableSkins.Contains(skin.skinName))
            .ToArray();
        UpdateView();
    }

    public void UpdateView()
    {
        foreach (Transform child in _itemsContainer.transform)
        {
            Destroy(child.gameObject);
        }
        
        var views = new List<InventorySkinView>(_skinsData.Length);

        foreach (var skin in _skinsData)
        {
            var instance = Instantiate(_skinViewPrefab, _itemsContainer.transform);
            instance.AttachData(skin);
            views.Add(instance);
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRect.GetComponent<RectTransform>());
    }
}