using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

[SelectionBase]
public abstract class WeaponViewBase : NetworkBehaviour
{
    [SerializeField] private Animator _weaponAnimator;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private WeaponRecoilConfig _weaponRecoilConfig;
    [SerializeField] private AudioClip _shotSound;
    [SerializeField] private Transform _leftHandIKPosition;
    [SerializeField] private Transform _rightHandIKPosition;
    [SerializeField] private AnimatorOverrideController _weaponHolderOverrideController;
    
    [Inject] private PlayerCameraRecoil _cameraRecoil;
    [Inject] private PlayerAnimator _playerAnimator;
    
    private WeaponConfigBase _weaponConfigBase;
    private AudioSource _audioSource;
    private GameObject _playerImpactEffect;
    
    public AnimatorOverrideController AnimatorOverrideController => _weaponHolderOverrideController;
    public Transform LeftHandPosition => _leftHandIKPosition;
    public Transform RightHandPosition => _rightHandIKPosition;
    public WeaponConfigBase WeaponConfig => _weaponConfigBase;
    public Transform Muzzle => _muzzle;
    protected Animator WeaponAnimator => _weaponAnimator;
    protected PlayerAnimator PlayerAnimator => _playerAnimator;

    public void Initialize(WeaponConfigBase weaponConfigBase)
    {
        _weaponConfigBase = weaponConfigBase;

        if (weaponConfigBase.PlayerHitEffect != null)
        {
            _playerImpactEffect = weaponConfigBase.PlayerHitEffect.gameObject;
        }
        
        _audioSource = GetComponent<AudioSource>();
    }

    public virtual void ShowMuzzleFlashEffect() { }
    
    public abstract void ShowEnvironmentImpactEffect(RaycastHit hit);

    public virtual void ShowPlayerImpactEffect(RaycastHit hit)
    {
        ServerSpawner.InstantiateObject(_playerImpactEffect, hit.point, Quaternion.identity);
    }
    
    public virtual void PlayShootAnimation() { }

    public virtual void PlayReloadAnimation()
    {
        _playerAnimator.PlayReloadAnimation();
    }

    public virtual void PerformRecoil()
    {
        _cameraRecoil.Perform(_weaponRecoilConfig);
    }

    public virtual void PlayShotSound()
    {
        _audioSource.clip = _shotSound;
        _audioSource.PlayOneShot(_shotSound);
    }
    
    public virtual void SlideWeapon() { }
}
