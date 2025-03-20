using Mirror;
using UnityEngine;

public class RevolverView : WeaponViewBase
{
    
    public override void ShowMuzzleFlashEffect()
    {
        if (isServer) 
        {
            Debug.Log("Client view shoot");
            var weapon = Instantiate(WeaponConfig.MuzzleFlash, Muzzle.position, transform.rotation);
            NetworkServer.Spawn(weapon.gameObject);
        }
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
        throw new System.NotImplementedException();
    }

    public override void PlayReloadAnimation()
    {
        throw new System.NotImplementedException();
    }
}