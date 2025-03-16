public class ShotgunModel : WeaponModelBase
{
    public int PelletСount { get; private set; }
    
    public ShotgunModel(ShotgunConfig config) : base(config)
    {
        PelletСount = config.PelletСount;
    }
}