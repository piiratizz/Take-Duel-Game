using Mirror;
using UnityEngine;

public class RevolverPresenter : WeaponPresenterBase
{
    private WeaponRaycaster _raycaster;
    
    public RevolverPresenter(RevolverModel model, RevolverView view) : base(model, view)
    {
        _raycaster = new WeaponRaycaster(PlayerCameraRoot.RaycastPosition);
    }

    public override void Shoot()
    {
        View.PlayShootAnimation();
        View.ShowMuzzleFlashEffect();
        
        var raycastResult = _raycaster.TryHitForward(out IHitPerformer performer);
        
        if(!raycastResult) return;
        performer?.PerformHit(new HitContext(10));
    }
    
    
    public override void Reload()
    {
        throw new System.NotImplementedException();
    }

   
}