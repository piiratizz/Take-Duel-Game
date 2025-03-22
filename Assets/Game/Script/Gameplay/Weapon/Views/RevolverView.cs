using Mirror;
using UnityEngine;

public class RevolverView : WeaponViewBase
{
    private static readonly int Shoot = Animator.StringToHash("Shoot");

    public override void ShowMuzzleFlashEffect()
    {
        ServerSpawner.InstantiateObject(WeaponConfig.MuzzleFlash.gameObject, Muzzle.position, transform.rotation);
    }

    public override void ShowEnvironmentImpactEffect(RaycastHit hit)
    {
        throw new System.NotImplementedException();
    }

    public override void ShowPlayerImpactEffect(RaycastHit hit)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayShootAnimation()
    {
        Animator.SetTrigger(Shoot);
    }

    public override void PlayReloadAnimation()
    {
        throw new System.NotImplementedException();
    }
}