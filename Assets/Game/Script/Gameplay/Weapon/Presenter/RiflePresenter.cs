using UnityEngine;
using UnityEngine.Events;

public class RiflePresenter : WeaponPresenterBase, ISlideRequireable
{
    private WeaponRaycaster _raycaster;
    
    public RiflePresenter(RifleModel model, RifleView view) : base(model, view)
    {
        _raycaster = new WeaponRaycaster(PlayerCameraRoot.Camera);
    }
    
    public override void Shoot()
    {
        View.ShowMuzzleFlashEffect();
        if(!_raycaster.TryHit(out IHitPerformer hit)) return;
    }

    public override void Reload()
    {
        
    }

    public void Slide()
    {
        throw new System.NotImplementedException();
    }
}