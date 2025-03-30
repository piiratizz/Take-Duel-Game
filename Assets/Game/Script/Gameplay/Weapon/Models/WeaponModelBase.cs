using R3;

public abstract class WeaponModelBase
{
    private readonly ReactiveProperty<int> _clipAmmo;
    private readonly ReactiveProperty<int> _totalAmmo;
    public int FireRate { get; private set; }
    public int PlayerDamage { get; private set; }
    public int EnvironmentDamage { get; private set; }
    
    public Observable<int> ClipAmmo => _clipAmmo;
    public Observable<int> TotalAmmo => _totalAmmo;

    protected WeaponModelBase(WeaponConfigBase config)
    {
        _clipAmmo = new ReactiveProperty<int>(config.ClipAmmo);
        _totalAmmo = new ReactiveProperty<int>(config.TotalAmmo);
        FireRate = config.FireRate;
        PlayerDamage = config.PlayerDamage;
        EnvironmentDamage = config.EnvironmentDamage;
    }
}