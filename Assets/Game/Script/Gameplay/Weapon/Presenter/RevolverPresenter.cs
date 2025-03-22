using Mirror;
using UnityEngine;

public class RevolverPresenter : WeaponPresenterBase
{
    private WeaponRaycaster _raycaster;
    
    public RevolverPresenter(RevolverModel model, RevolverView view) : base(model, view)
    {
        _raycaster = new WeaponRaycaster(PlayerCameraRoot.Camera);
    }

    public override void Shoot()
    {
        View.PlayShootAnimation();
        View.ShowMuzzleFlashEffect();
        
        var raycastResult = _raycaster.TryHit(out IHitPerformer hit);
        Debug.Log(raycastResult);
        if(!raycastResult) return;
        
        Debug.Log($"SERVER HIT");
        hit.PerformHit(new HitContext(Model.PlayerDamage));
    }
    
    
    public override void Reload()
    {
        throw new System.NotImplementedException();
    }

   
}