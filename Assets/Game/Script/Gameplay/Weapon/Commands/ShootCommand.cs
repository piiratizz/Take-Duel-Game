public class ShootCommand : IWeaponCommand
{
    public void Execute(WeaponPresenterBase weapon)
    {
        weapon.Shoot();
    }
}