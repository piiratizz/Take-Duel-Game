using Mirror;
using UnityEngine;

[SelectionBase]
public abstract class WeaponViewBase : NetworkBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private WeaponRecoilConfig _weaponRecoilConfig;
    [SerializeField] private AudioClip _shotSound;
    [SerializeField] private GameObject _playerImpactEffect;
    
    private WeaponConfigBase _weaponConfigBase;
    private HandsIKConfig _handsIKConfig;
    private PlayerCameraRecoil _cameraRecoil;
    private AudioSource _audioSource;
    public HandsIKConfig HandsConfig => _handsIKConfig;
    public WeaponConfigBase WeaponConfig => _weaponConfigBase;
    public Transform Muzzle => _muzzle;
    protected Animator Animator => _animator;

    public void Initialize(HandsIKConfig handConfig, WeaponConfigBase weaponConfigBase, PlayerCameraRecoil cameraRecoil)
    {
        _cameraRecoil = cameraRecoil;
        _handsIKConfig = handConfig;
        _weaponConfigBase = weaponConfigBase;
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
