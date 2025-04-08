using Mirror;
using UnityEngine;

public class PlayerUIRoot : NetworkBehaviour
{
    [SerializeField] private PlayerHealthUI _playerHealthUI;
    [SerializeField] private Canvas _canvas;
    
    public void Initialize()
    {
        _canvas.worldCamera = PlayerCameraRoot.Camera;
    }
    
    [Command]
    public void CmdUpdateHealth(float newHealth)
    {
        RpcUpdateHealth(newHealth);
    }
    
    [ClientRpc]
    public void RpcUpdateHealth(float newHealth)
    {
        _playerHealthUI.UpdateHealth(newHealth);
    }
}
