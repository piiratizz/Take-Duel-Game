public class ShotgunPresenter : WeaponPresenterBase
{
    public override void Shoot()
    {
        throw new System.NotImplementedException();
    }

    public override void Reload()
    {
        throw new System.NotImplementedException();
    }

    public ShotgunPresenter(ShotgunModel model, ShotgunView view) : base(model, view)
    {
    }
}