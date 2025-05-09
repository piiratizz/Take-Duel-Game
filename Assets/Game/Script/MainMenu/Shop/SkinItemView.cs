using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkinItemView : MonoBehaviour
{
    [SerializeField] private RawImage _previewImage;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _buyButton;
    
    public SkinData SkinData { get; private set; }
    public UnityEvent<SkinData> BuyEvent;
    
    public void AttachData(SkinData data)
    {
        SkinData = data;
        BuyEvent = new UnityEvent<SkinData>();
        
        _previewImage.texture = data.previewImage;
        _priceText.text = data.price.ToString();

        _buyButton.onClick = new Button.ButtonClickedEvent();
        _buyButton.onClick.AddListener(OnBuyClick);
    }

    private void OnBuyClick()
    {
        BuyEvent.Invoke(SkinData);
    }
}
