using Mirror;
using UnityEngine;

public class PlayerSkinChanger : NetworkBehaviour
{
    [SerializeField] private Transform _skinHolder;
    
    public void SetSkin(string skinName)
    {
        foreach (Transform skinObject in _skinHolder)
        {
            skinObject.gameObject.SetActive(skinObject.name == skinName);
        }
    }
}