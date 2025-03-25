using System;
using Mirror;
using R3;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(WeaponViewBase))]
public class WeaponController : NetworkBehaviour
{
    [SerializeField] private WeaponConfigBase _config;
    [SerializeField] private HandsIKConfig _handsIKConfig;
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private bool _isSlideRequired;
    
    private ISlideRequireable _slideRequireable;
    private WeaponViewBase _view;
    private WeaponPresenterBase _presenter;
    private WeaponModelBase _model;

    private WeaponHudUI _weaponHud;
    private IDisposable _clipAmmoFieldSubscription;
    private IDisposable _totalAmmoFieldSubscription;

    private bool _initialized;
    
    public void Initialize()
    {
        _view = GetComponent<WeaponViewBase>();
        _view.Initialize(_handsIKConfig, _config);


        switch (_weaponType)
        {
            case WeaponType.Revolver:
                _model = new RevolverModel(_config as RevolverConfig);
                _presenter = new RevolverPresenter(_model as RevolverModel, _view as RevolverView);
                break;
            case WeaponType.Shotgun:
                _model = new ShotgunModel(_config as ShotgunConfig);
                _presenter = new ShotgunPresenter(_model as ShotgunModel, _view as ShotgunView);
                break;
            case WeaponType.Rifle:
                _model = new RifleModel(_config as RifleConfig);
                _presenter = new RiflePresenter(_model as RifleModel, _view as RifleView);
                break;
        }

        if (_isSlideRequired)
        {
            _slideRequireable = _presenter as ISlideRequireable;
        }

        _initialized = true;
        if (!isLocalPlayer) return;
        _weaponHud = ContainerHolder.Resolve<GameplayUIRoot>().WeaponHud;
    }

    public void Reload()
    {
        _presenter.Reload();
    }

    public void Shoot()
    {
        _presenter.Shoot();
    }

    public void Slide()
    {
        if (_isSlideRequired)
        {
            _slideRequireable.Slide();
        }
    }

    public void SubscribeUI()
    {
        if (!isLocalPlayer || !_initialized) return;
        
        _clipAmmoFieldSubscription = _model.ClipAmmo.Subscribe(value =>
        {
            _weaponHud.ClipAmmo.text = value.ToString();
            Debug.Log($"CLIP AMMO CHANGED {value}");
        });
        
        _totalAmmoFieldSubscription = _model.TotalAmmo.Subscribe(value =>
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