using UnityEngine;
using UnityEngine.Events;

public class RiflePresenter : WeaponPresenterBase, ISlideRequireable
{
    private readonly WeaponRaycaster _raycaster;
    private bool _isSlided;
    
    public RiflePresenter(RifleModel model, RifleView view) : base(model, view)
    {
        _raycaster = new WeaponRaycaster(PlayerCameraRoot.RaycastPosition);
    }
    
    public override void Shoot()
    {
        if(Model.ClipAmmoCount <= 0 || !_isSlided) return;
        _isSlided = false;
        
        View.ShowMuzzleFlashEffect();
        Model.TakeShot();
        var raycastResult = _raycaster.TryHitForward(out IHitPerformer performer);
        
        if(!raycastResult) return;
        performer?.PerformHit(new HitContext(Model.PlayerDamage));
    }

    public override void Reload()
    {
        Model.Reload();
    }

    public void Slide()
    {
        _isSlided = true;
    }
}