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
    [SerializeField] private Transform _leftHandIKPosition;
    [SerializeField] private Transform _rightHandIKPosition;
    [SerializeField] private AnimatorOverrideController _weaponHolderOverrideController;
    [SerializeField] private AudioSource _audioSource;
    
    [Inject] private PlayerCameraRecoil _cameraRecoil;
    [Inject] private PlayerAnimator _playerAnimator;
    
    private WeaponConfigBase _weaponConfigBase;
    
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
        if (_weaponConfigBase.ShotSound.Clip != null)
        {
            _audioSource.PlayOneShot(_weaponConfigBase.ShotSound.Clip, _weaponConfigBase.ShotSound.Volume);
        }
    }

    public virtual void PlayAttachSound()
    {
        if (_weaponConfigBase.ShotSound.Clip != null)
        {
            _audioSource.PlayOneShot(_weaponConfigBase.AttachSound.Clip, _weaponConfigBase.AttachSound.Volume);
        }
    }
    
    public virtual void SlideWeapon() { }
}
