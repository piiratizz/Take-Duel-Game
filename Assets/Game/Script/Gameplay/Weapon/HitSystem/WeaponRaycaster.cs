using UnityEngine;

public class WeaponRaycaster
{
    private Transform _raycastSource;

    public RaycastHit HitInfo { get; private set; }
    
    public WeaponRaycaster(Transform raycastSource)
    {
        _raycastSource = raycastSource;
    }
    
    public bool TryHitForward(out IHitPerformer hitPermormer)
    {
        hitPermormer = null;
        var ray = new Ray(_raycastSource.position, _raycastSource.forward);
        if (!Physics.Raycast(ray: ray, out var hit)) return false;
        HitInfo = hit;
        if (!hit.collider.TryGetComponent(out hitPermormer)) return false;
        
        Debug.Log("Hitted object " + hit.collider.gameObject.name);
        return true;
    }
}