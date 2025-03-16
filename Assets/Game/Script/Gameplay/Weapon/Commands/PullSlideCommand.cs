using System;

public class PullSlideCommand : IWeaponCommand
{
    public void Execute(WeaponPresenterBase weapon)
    {
        var rifle = weapon as RiflePresenter;
        
        if (rifle == null)
        {
            throw new InvalidCastException($"{rifle} cast error");
        }
        
        rifle.PullSlide();
    }
}