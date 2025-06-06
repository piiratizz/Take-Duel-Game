using UnityEngine;

[CreateAssetMenu(menuName = "Skins/Data", fileName = "SkinData")]
public class SkinData : ScriptableObject
{
    public string skinName;
    public Texture2D previewImage;
    public int price;
}
