using Mirror;
using UnityEngine;

public class RevolverPresenter : WeaponPresenterBase
{
    private WeaponRaycaster _weapon;
    
    public RevolverPresenter(RevolverModel model, RevolverView view) : base(model, view)
    {
        _weapon = new WeaponRaycaster(PlayerCameraRoot.Camera);
    }

    public override void Shoot()
    {
        Debug.Log("Server presenter shoot");
        if(!_weapon.TryHit(out var hit)) return;
        Debug.Log(hit.collider.gameObject.name);
        View.ShowMuzzleFlashEffect();
    }

    public override void Reload()
    {
        throw new System.NotImplementedException();
    }

   
}