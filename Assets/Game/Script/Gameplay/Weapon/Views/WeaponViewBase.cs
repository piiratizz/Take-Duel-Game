using Mirror;
using UnityEngine;

public abstract class WeaponViewBase : MonoBehaviour
{
    [SerializeField] private Transform _muzzle;
    [SerializeField] private WeaponConfigBase _config;
    [SerializeField] private HandsIKConfig _handsIKConfig;
    
    public HandsIKConfig HandsConfig => _handsIKConfig;

    public abstract void ShowMuzzleFlashEffect();
    
    public abstract void ShowEnvironmentImpactEffect();
    public abstract void ShowPlayerImpactEffect();
    
    public abstract void PlayShootAnimation();
    public abstract void PlayReloadAnimation();
}