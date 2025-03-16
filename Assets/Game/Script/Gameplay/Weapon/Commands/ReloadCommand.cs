public class ReloadCommand : IWeaponCommand
{
    public void Execute(WeaponPresenterBase weapon)
    {
        weapon.Reload();
    }
}