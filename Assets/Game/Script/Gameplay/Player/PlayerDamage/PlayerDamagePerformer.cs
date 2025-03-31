using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDamagePerformer : NetworkBehaviour, IDamageable
{
    [HideInInspector] public UnityEvent<HitContext> HitEvent = new UnityEvent<HitContext>();
    
    public void PerformHit(HitContext ctx)
    {
        HitEvent.Invoke(ctx);
    }

    public NetworkIdentity GetNetworkIdentity()
    {
        return netIdentity;
    }
}