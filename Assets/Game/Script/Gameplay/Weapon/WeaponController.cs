using System;
using Cysharp.Threading.Tasks;
using Mirror;
using R3;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(WeaponViewBase))]
public class WeaponController : NetworkBehaviour
{
    [SerializeField] private WeaponConfigBase _config;
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private bool _isSlideRequired;
    [SerializeField] private LayerMask _layerMask;

    [Inject] private PlayerCameraRoot _playerCameraRoot;
    [Inject] private LagCompensator _compensator;
    
    private WeaponViewBase _view;
    private WeaponModelBase _model;

    private WeaponHudUI _weaponHud;
    private IDisposable _clipAmmoFieldSubscription;
    private IDisposable _totalAmmoFieldSubscription;

    private bool _initialized;
    private bool _slided;
    [SyncVar] private bool _reloaded = true;
    
    private WeaponRaycaster _raycaster;
    private float _elapsedTimeAfterShot;

    public bool Reloaded => _reloaded;
    
    public void Initialize()
    {
        _raycaster = new WeaponRaycaster(_playerCameraRoot.RaycastPosition);
        
        _view = GetComponent<WeaponViewBase>();
        _view.Initialize(_config);


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

    [Client]
    public void Reload()
    {
        if(!_model.IsNeedReload()) return;
        
        CmdReload();
    }
    
    [Command]
    private void CmdReload()
    {
        _model.Reload();
        RpcReload();
    }
    
    [ClientRpc]
    private void RpcReload()
    {
        _view.PlayReloadAnimation();
        if(isServer) return;
        _model.Reload();
    }

    [Client]
    public void Shoot()
    {
        if(_raycaster.TryHitForward(out IHitPerformer hitObject))
            CmdShoot(_playerCameraRoot.RaycastPosition.position, _raycaster.HitInfo.point);
        else
            CmdShoot(_playerCameraRoot.RaycastPosition.position, Vector3.zero);
        
        PerformClientShootView();
    }
    
    [Command]
    private void CmdShoot(Vector3 origin, Vector3 target)
    {
        if (!IsCanShoot()) return;
        
        _elapsedTimeAfterShot = 0;
        _model.TakeShot();
        LateRpcShoot();

        if (target == Vector3.zero) return;
        
        var lagCompensationResult =
            _compensator.RaycastCheck
            (
                this.connectionToClient,
                origin,
                target,
                10,
                _layerMask,
                out var hit
            );
        
        //DEBUG
        _gizmosDebugSpherePosition = target;
        _gizmosDebugRayOriginPosition = origin;
        _gizmosDebugRayTargetPosition = target;
        //
        
        Debug.Log(lagCompensationResult);
        if(lagCompensationResult)
            Debug.Log(hit.collider.gameObject.name);
        
        if(!lagCompensationResult) return;

        hit.collider.TryGetComponent<IHitPerformer>(out var performer);
        
        performer?.PerformHit(new HitContext(_model.PlayerDamage, performer.GetNetworkIdentity()));
    }
    
    [ClientRpc]
    private void LateRpcShoot()
    {
        if (!isLocalPlayer)
        {
            _view.PlayShotSound();
            _view.ShowMuzzleFlashEffect();
        }
        
        if(!netIdentity.isServer)
            _model.TakeShot();
    }
    
    [Client]
    private void PerformClientShootView()
    {
        if (!IsCanShoot()) return;
        
        _view.PlayShotSound();
        _view.PerformRecoil();
        _view.ShowMuzzleFlashEffect();
        _view.PlayShootAnimation();
        
        if(!netIdentity.isServer)
            _elapsedTimeAfterShot = 0;
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

    public void OnReloadComplete() => _reloaded = true;
    public void OnReloadStarted() => _reloaded = false;
    
    private bool IsCanShoot()
    {
        return _model.ClipAmmoCount > 0 && _elapsedTimeAfterShot >= 60f / _config.FireRate && _reloaded;
    }


    private Vector3 _gizmosDebugSpherePosition;
    private Vector3 _gizmosDebugRayOriginPosition;
    private Vector3 _gizmosDebugRayTargetPosition;
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_gizmosDebugSpherePosition, 0.1f);
        Gizmos.DrawLine(_gizmosDebugRayOriginPosition, _gizmosDebugRayTargetPosition);
    }
}