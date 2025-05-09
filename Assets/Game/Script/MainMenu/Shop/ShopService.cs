using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopService : MonoBehaviour
{
    [SerializeField] private SkinItemView _skinViewPrefab;
    [SerializeField] private SkinData[] _skinsData;
    [SerializeField] private HorizontalLayoutGroup _itemsContainer;
    [SerializeField] private ScrollRect _scrollRect;
    
    public void Start()
    {
        InitializeSkins();
    }

    private void InitializeSkins()
    {
        var views = new List<SkinItemView>(_skinsData.Length);

        foreach (var skin in _skinsData)
        {
            var instance = Instantiate(_skinViewPrefab, _itemsContainer.transform);
            instance.AttachData(skin);
            views.Add(instance);
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRect.GetComponent<RectTransform>());
    }
}
