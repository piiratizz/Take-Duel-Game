using Mirror;
using UnityEngine;

public abstract class WeaponViewBase : NetworkBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _muzzle;

    private WeaponConfigBase _weaponConfigBase;
    private HandsIKConfig _handsIKConfig;
    
    public HandsIKConfig HandsConfig => _handsIKConfig;
    public WeaponConfigBase WeaponConfig => _weaponConfigBase;
    public Transform Muzzle => _muzzle;
    protected Animator Animator => _animator;

    public void Initialize(HandsIKConfig handConfig, WeaponConfigBase weaponConfigBase)
    {
        _handsIKConfig = handConfig;
        _weaponConfigBase = weaponConfigBase;
    }

    public virtual void ShowMuzzleFlashEffect() {}
    
    public abstract void ShowEnvironmentImpactEffect(RaycastHit hit);
    public abstract void ShowPlayerImpactEffect(RaycastHit hit);
    
    public abstract void PlayShootAnimation();
    public abstract void PlayReloadAnimation();
}