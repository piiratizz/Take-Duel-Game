using Mirror;
using UnityEngine;

public class PlayerSkinChanger : NetworkBehaviour
{
    [SerializeField] private Transform _skinsWithHeadHolder;
    [SerializeField] private Transform _skinsNoHeadHolder;

    private Transform _skinsHolder;
    
    public void SetSkin(string skinName)
    {
        _skinsHolder.gameObject.SetActive(true);
        
        foreach (Transform skinObject in _skinsHolder)
        {
            skinObject.gameObject.SetActive(skinObject.name == skinName);
        }
    }

    public void SetNoHeadMesh()
    {
        _skinsHolder = _skinsNoHeadHolder;
    }

    public void SetHeadMesh()
    {
        _skinsHolder = _skinsWithHeadHolder;
    }
}