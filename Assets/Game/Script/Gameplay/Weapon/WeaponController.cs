using System;
using Cysharp.Threading.Tasks;
using Mirror;
using R3;
using UnityEngine;

[RequireComponent(typeof(WeaponViewBase))]
public class WeaponController : NetworkBehaviour
{
    [SerializeField] private WeaponConfigBase _config;
    [SerializeField] private HandsIKConfig _handsIKConfig;
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private bool _isSlideRequired;
    
    private WeaponViewBase _view;
    private WeaponModelBase _model;

    private WeaponHudUI _weaponHud;
    private IDisposable _clipAmmoFieldSubscription;
    private IDisposable _totalAmmoFieldSubscription;

    private bool _initialized;
    private bool _slided;
    
    private WeaponRaycaster _raycaster;
    private float _elapsedTimeAfterShot;
    
    public void Initialize(PlayerCameraRecoil cameraRecoil, PlayerAnimator playerAnimator)
    {
        _raycaster = new WeaponRaycaster(PlayerCameraRoot.RaycastPosition);
        
        _view = GetComponent<WeaponViewBase>();
        _view.Initialize(_config, cameraRecoil, playerAnimator);


        switch (_weaponType)
        {
            case WeaponType.Revolver:
                _model = new RevolverModel(_config as RevolverConfig);
                break;
            case WeaponType.Shotgun:
                _model = new ShotgunModel(_config as ShotgunConfig);
                break;
            case WeaponType.Rifle:
                _model = new RifleModel(_config as RifleConfig);
                break;
        }

        if (_isSlideRequired)
        {
        }

        _initialized = true;
        if (!isLocalPlayer) return;
        _weaponHud = ContainerHolder.Resolve<GameplayUIRoot>().WeaponHud;
    }

    private void Update()
    {
        _elapsedTimeAfterShot += Time.deltaTime;
    }

    [Command]
    public void CmdReload()
    {
        _model.Reload();
        RpcReload();
    }
    
    [ClientRpc]
    private void RpcReload()
    {
        if(isServer) return;
        _model.Reload();
    }

    [Command]
    public void CmdShoot()
    {
        if (_model.ClipAmmoCount <= 0 || _elapsedTimeAfterShot < 60f / _config.FireRate) return;

        _elapsedTimeAfterShot = 0;
        _view.PlayShootAnimation();
        _view.ShowMuzzleFlashEffect();
        _model.TakeShot();
        RpcShoot();
        
        var raycastResult = _raycaster.TryHitForward(out IHitPerformer hitObject);
        var hitInfo = _raycaster.HitInfo;
        if(!raycastResult) return;
        hitObject?.PerformHit(new HitContext(_model.PlayerDamage, hitObject.GetNetworkIdentity()));
        _view.ShowPlayerImpactEffect(hitInfo);
    }
    
    [ClientRpc]
    private void RpcShoot()
    {
        _view.PerformRecoil();
        _view.PlayShotSound();
        
        if(!netIdentity.isServer)
            _model.TakeShot();
    }

    public void Slide()
    {
        if(!_isSlideRequired) return;

        _slided = true;
    }

    public void SubscribeUI()
    {
        if (!isLocalPlayer || !_initialized) return;
        
        _clipAmmoFieldSubscription = _model.ClipAmmoProperty.Subscribe(value =>
        {
            _weaponHud.ClipAmmo.text = value.ToString();
            Debug.Log($"CLIP AMMO CHANGED {value}");
        });
        
        _totalAmmoFieldSubscription = _model.TotalAmmoProperty.Subscribe(value =>
        {
            _weaponHud.TotalAmmo.text = value.ToString();
            Debug.Log($"TOTAL AMMO CHANGED {value}");
        });
        
        Debug.Log("UI SUBSCRIBED");
    }

    public void UnsubscribeUI()
    {
        if (!isLocalPlayer || !_initialized) return;
        
        _clipAmmoFieldSubscription?.Dispose();
        _totalAmmoFieldSubscription?.Dispose();
    }
}