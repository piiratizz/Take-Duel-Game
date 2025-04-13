using UnityEngine;

public class ShotgunView : WeaponViewBase
{
    
    public override void ShowMuzzleFlashEffect()
    {
        Instantiate(WeaponConfig.MuzzleFlash.gameObject, Muzzle.position, transform.rotation);
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