using Mirror;

public struct HitContext
{
    public float BulletDamage;
    public NetworkIdentity Target; 
    
    public HitContext(float bulletDamage)
    {
        BulletDamage = bulletDamage;
        Target = null;
    }

    public HitContext(float bulletDamage, NetworkIdentity target) : this(bulletDamage)
    {
        Target = target;
    }
}