using UnityEngine;

public class ShotgunPresenter : WeaponPresenterBase
{
    private WeaponRaycaster _raycaster;
    
    public ShotgunPresenter(ShotgunModel model, ShotgunView view) : base(model, view)
    {
        _raycaster = new WeaponRaycaster(PlayerCameraRoot.RaycastPosition);
    }
    
    public override void Shoot()
    {
        View.ShowMuzzleFlashEffect();
        //if(!_raycaster.TryHit(out IHitPerformer hit)) return;
        
    }

    public override void Reload()
    {
        throw new System.NotImplementedException();
    }
}