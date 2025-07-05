using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity), typeof(Rigidbody))]
public class DestroyableEntity : NetworkBehaviour, IDestroyable
{
    [SerializeField] private GameObject defaultObject;
    [SerializeField] private GameObject celledObject;
    [SerializeField] private float objectMass;
    
    private NetworkIdentity _cachedIdentityComponent;
    private Rigidbody _cachedRigidBodyComponent;
    private Collider _cachedMainColliderComponent;
    
    private const float ForceMultiplier = 1000f;
    private const float RawRadius = 100f;
    
    private void Awake()
    {
        _cachedIdentityComponent = GetComponent<NetworkIdentity>();
        _cachedRigidBodyComponent = GetComponent<Rigidbody>();
        _cachedMainColliderComponent = GetComponent<Collider>();
        
        celledObject.SetActive(false);
        defaultObject.SetActive(true);
    }

    public void PerformHit(HitContext ctx)
    {
        RpcDestroyObject(ctx);
    }
    
    [ClientRpc]
    private void RpcDestroyObject(HitContext ctx)
    {
        defaultObject.SetActive(false);
        celledObject.SetActive(true);

        _cachedMainColliderComponent.enabled = false;
        _cachedRigidBodyComponent.AddExplosionForce(ctx.BulletDamage * ForceMultiplier, transform.position, RawRadius / objectMass);
    }
    
    public NetworkIdentity GetNetworkIdentity()
    {
        return _cachedIdentityComponent;
    }
}