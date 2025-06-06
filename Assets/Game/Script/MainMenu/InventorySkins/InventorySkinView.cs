using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventorySkinView : MonoBehaviour
{
    [SerializeField] private RawImage _previewImage;
    [SerializeField] private Button _equipButton;
    
    public SkinData SkinData { get; private set; }
    public UnityEvent<SkinData> EquipEvent;
    
    public void AttachData(SkinData data)
    {
        SkinData = data;
        EquipEvent = new UnityEvent<SkinData>();
        
        _previewImage.texture = data.previewImage;

        _equipButton.onClick = new Button.ButtonClickedEvent();
        _equipButton.onClick.AddListener(OnEquipClick);
    }

    private void OnEquipClick()
    {
        EquipEvent.Invoke(SkinData);
    }
}