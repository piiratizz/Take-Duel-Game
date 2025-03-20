using R3;

public abstract class WeaponModelBase
{
    public ReactiveProperty<int> ClipAmmo { get; private set; }
    public ReactiveProperty<int> TotalAmmo { get; private set; }
    public int FireRate { get; private set; }
    public int PlayerDamage { get; private set; }
    public int EnvironmentDamage { get; private set; }

    protected WeaponModelBase(WeaponConfigBase config)
    {
        ClipAmmo = new ReactiveProperty<int>(config.ClipAmmo);
        TotalAmmo = new ReactiveProperty<int>(config.TotalAmmo);
        FireRate = config.FireRate;
        PlayerDamage = config.PlayerDamage;
        EnvironmentDamage = config.EnvironmentDamage;
    }
}