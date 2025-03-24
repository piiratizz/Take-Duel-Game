using UnityEngine;

public class ShotgunPresenter : WeaponPresenterBase
{
    private WeaponRaycaster _raycaster;
    
    public ShotgunPresenter(ShotgunModel model, ShotgunView view) : base(model, view)
    {
    }
    
    public override void Shoot()
    {
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