using Mirror;
using UnityEngine;

public class RevolverView : WeaponViewBase
{
    private static readonly int Shoot = Animator.StringToHash("Shoot");

    public override void ShowMuzzleFlashEffect()
    {
        //ServerSpawner.InstantiateObject(WeaponConfig.MuzzleFlash.gameObject, Muzzle.position, transform.rotation);
       Instantiate(WeaponConfig.MuzzleFlash.gameObject, Muzzle.position, transform.rotation);
    }

    public override void ShowEnvironmentImpactEffect(RaycastHit hit)
    {
        throw new System.NotImplementedException();
    }
    
    public override void PlayShootAnimation()
    {
        WeaponAnimator.SetTrigger(Shoot);
        PlayerAnimator.PlayShootAnimation();
    }
}