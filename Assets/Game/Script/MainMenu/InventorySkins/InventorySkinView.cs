using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventorySkinView : MonoBehaviour
{
    [SerializeField] private RawImage _previewImage;
    [SerializeField] private Button _equipButton;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private string _equipButtonText;
    [SerializeField] private string _deEquipButtonText;
    
    public SkinData SkinData { get; private set; }
    public UnityEvent<SkinData, InventorySkinView> EquipEvent;
    
    public void AttachData(SkinData data)
    {
        SkinData = data;
        EquipEvent = new UnityEvent<SkinData, InventorySkinView>();
        
        _previewImage.texture = data.previewImage;

        _equipButton.onClick = new Button.ButtonClickedEvent();
        _equipButton.onClick.AddListener(OnEquipClick);
    }

    private void OnEquipClick()
    {
        EquipEvent.Invoke(SkinData, this);
    }

    public void OnSkinEquip()
    {
        _buttonText.text = _equipButtonText;
    }
    
    public void OnSkinDeEquip()
    {
        _buttonText.text = _deEquipButtonText;
    }
}