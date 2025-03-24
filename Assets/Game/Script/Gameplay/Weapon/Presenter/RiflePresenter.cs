using UnityEngine;
using UnityEngine.Events;

public class RiflePresenter : WeaponPresenterBase, ISlideRequireable
{
    private WeaponRaycaster _raycaster;
    
    public RiflePresenter(RifleModel model, RifleView view) : base(model, view)
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
        
    }

    public void Slide()
    {
        throw new System.NotImplementedException();
    }
}