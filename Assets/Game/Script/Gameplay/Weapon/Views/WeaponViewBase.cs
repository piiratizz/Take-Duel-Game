using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

[SelectionBase]
public abstract class WeaponViewBase : NetworkBehaviour
{
    [SerializeField] private Animator _weaponAnimator;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private WeaponRecoilConfig _weaponRecoilConfig;
    [SerializeField] private AudioClip _shotSound;
    [SerializeField] private GameObject _playerImpactEffect;
    [SerializeField] private Transform _leftHandIKPosition;
    [SerializeField] private Transform _rightHandIKPosition;
    [SerializeField] private AnimatorOverrideController _weaponHolderOverrideController;
    
    private WeaponConfigBase _weaponConfigBase;
    private PlayerCameraRecoil _cameraRecoil;
    private AudioSource _audioSource;
    private PlayerAnimator _playerAnimator;
    
    public AnimatorOverrideController AnimatorOverrideController => _weaponHolderOverrideController;
    public Transform LeftHandPosition => _leftHandIKPosition;
    public Transform RightHandPosition => _rightHandIKPosition;
    public WeaponConfigBase WeaponConfig => _weaponConfigBase;
    public Transform Muzzle => _muzzle;
    protected Animator WeaponAnimator => _weaponAnimator;
    protected PlayerAnimator PlayerAnimator => _playerAnimator;

    public void Initialize(WeaponConfigBase weaponConfigBase, PlayerCameraRecoil cameraRecoil, PlayerAnimator playerAnimator)
    {
        _cameraRecoil = cameraRecoil;
        _weaponConfigBase = weaponConfigBase;
        _playerAnimator = playerAnimator;
        _audioSource = GetComponent<AudioSource>();
    }

    public virtual void ShowMuzzleFlashEffect() {}
    
    public abstract void ShowEnvironmentImpactEffect(RaycastHit hit);

    public virtual void ShowPlayerImpactEffect(RaycastHit hit)
    {
        ServerSpawner.InstantiateObject(_playerImpactEffect, hit.point, Quaternion.identity);
    }
    
    public abstract void PlayShootAnimation();
    public abstract void PlayReloadAnimation();

    public virtual void PerformRecoil()
    {
        _cameraRecoil.Perform(_weaponRecoilConfig);
    }

    public virtual void PlayShotSound()
    {
        _audioSource.clip = _shotSound;
        _audioSource.Play();
    }
    
    public virtual void SlideWeapon() { }
}
