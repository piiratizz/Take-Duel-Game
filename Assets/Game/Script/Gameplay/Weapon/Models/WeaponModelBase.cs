using R3;
using UnityEngine;

public abstract class WeaponModelBase
{
    private readonly ReactiveProperty<int> _clipAmmo;
    private readonly ReactiveProperty<int> _totalAmmo;
    public int FireRate { get; private set; }
    public int PlayerDamage { get; private set; }
    public int EnvironmentDamage { get; private set; }
    public int ClipSize { get; private set; }
    public Observable<int> ClipAmmoProperty => _clipAmmo;
    public int ClipAmmoCount => _clipAmmo.Value;
    
    public Observable<int> TotalAmmoProperty => _totalAmmo;
    public int TotalAmmoCount => _totalAmmo.Value;

    protected WeaponModelBase(WeaponConfigBase config)
    {
        _clipAmmo = new ReactiveProperty<int>(config.ClipAmmo);
        _totalAmmo = new ReactiveProperty<int>(config.TotalAmmo);
        FireRate = config.FireRate;
        PlayerDamage = config.PlayerDamage;
        EnvironmentDamage = config.EnvironmentDamage;
        ClipSize = config.ClipAmmo;
    }

    public void TakeShot()
    {
        if(ClipAmmoCount <= 0) return;
        
        _clipAmmo.Value -= 1;
    }

    public void Reload()
    {
        if(TotalAmmoCount <= 0) return;

        var ammoToClip = Mathf.Clamp(ClipSize - ClipAmmoCount, 0, TotalAmmoCount);
        _clipAmmo.Value += ammoToClip;
        _totalAmmo.Value -= ammoToClip;
    }

}