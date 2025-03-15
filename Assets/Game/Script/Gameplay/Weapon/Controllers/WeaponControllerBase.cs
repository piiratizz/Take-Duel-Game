using UnityEngine.Events;

public abstract class WeaponControllerBase
{
    public UnityEvent PlayerHitEvent;
    public UnityEvent EnvironmentHitEvent;
    public UnityEvent ShootEvent;
    public UnityEvent ReloadEvent;
    
    public abstract void Shoot();
    public abstract void Reload();
}