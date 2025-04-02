using Mirror;
using UnityEngine;
using Zenject;

public class RevolverPresenter : WeaponPresenterBase
{
    private WeaponRaycaster _raycaster;
    
    public RevolverPresenter(RevolverModel model, RevolverView view) : base(model, view)
    {
        _raycaster = new WeaponRaycaster(PlayerCameraRoot.RaycastPosition);
    }

    public override void CmdShoot(NetworkIdentity netIdentity)
    {
        if (Model.ClipAmmoCount <= 0) return;
 
        View.PlayShootAnimation();
        View.ShowMuzzleFlashEffect();
        Model.TakeShot();
        
        var raycastResult = _raycaster.TryHitForward(out IHitPerformer hitObject);
        
        if(!raycastResult) return;
        hitObject?.PerformHit(new HitContext(Model.PlayerDamage, hitObject.GetNetworkIdentity()));
    }
    
    public override void RpcShoot(NetworkIdentity netIdentity)
    {
        if (Model.ClipAmmoCount <= 0) return;
        
        View.PerformRecoil();
        View.PlayShotSound();
        
        if(!netIdentity.isServer)
            Model.TakeShot();
    }
    
    
    public override void CmdReload(NetworkIdentity netIdentity)
    {
        Model.Reload();
    }

    public override void RpcReload(NetworkIdentity netIdentity)
    {
        Model.Reload();
    }

   
}