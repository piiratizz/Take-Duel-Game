using Mirror;
using UnityEngine;

public class ShotgunPresenter : WeaponPresenterBase
{
    private readonly WeaponRaycaster _raycaster;
    
    public ShotgunPresenter(ShotgunModel model, ShotgunView view) : base(model, view)
    {
        _raycaster = new WeaponRaycaster(PlayerCameraRoot.RaycastPosition);
    }
    
    public override void CmdShoot(NetworkIdentity netIdentity)
    {
        if(Model.ClipAmmoCount <= 0) return;
        
        View.ShowMuzzleFlashEffect();
        Model.TakeShot();
        
        var raycastResult = _raycaster.TryHitForward(out IHitPerformer performer);
        
        if(!raycastResult) return;
        performer?.PerformHit(new HitContext(Model.PlayerDamage));
    }

    public override void CmdReload(NetworkIdentity netIdentity)
    {
        Model.Reload();
    }
}