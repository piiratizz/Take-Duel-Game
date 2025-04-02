using UnityEngine;

public class ShotgunView : WeaponViewBase
{
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
        
    }

    public override void PlayReloadAnimation()
    {
        throw new System.NotImplementedException();
    }
}