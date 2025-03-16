using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponPresenterBase
{
    public UnityEvent<RaycastHit> PlayerHitEvent = new UnityEvent<RaycastHit>();
    public UnityEvent<RaycastHit> EnvironmentHitEvent = new UnityEvent<RaycastHit>();
    public UnityEvent ShootEvent = new UnityEvent();
    public UnityEvent ReloadEvent = new UnityEvent();
    
    public abstract void Shoot();
    public abstract void Reload();
}