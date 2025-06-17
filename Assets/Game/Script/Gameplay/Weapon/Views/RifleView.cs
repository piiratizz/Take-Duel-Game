using UnityEngine;

public class RifleView : WeaponViewBase
{
    public override void ShowMuzzleFlashEffect()
    {
        ServerSpawner.InstantiateObject(WeaponConfig.MuzzleFlash.gameObject, Muzzle.position, transform.rotation);
    }

    public override void ShowEnvironmentImpactEffect(RaycastHit hit)
    {
        throw new System.NotImplementedException();
    }


    public override void PlayShootAnimation()
    {
        PlayerAnimator.PlayShootAnimation();
    }
}