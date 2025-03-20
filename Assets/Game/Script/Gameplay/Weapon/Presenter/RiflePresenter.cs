using UnityEngine.Events;

public class RiflePresenter : WeaponPresenterBase, ISlideRequireable
{
    public RiflePresenter(RifleModel model, RifleView view) : base(model, view)
    {
    }
    
    public override void Shoot()
    {
        
    }

    public override void Reload()
    {
        
    }

    public void Slide()
    {
        throw new System.NotImplementedException();
    }
}