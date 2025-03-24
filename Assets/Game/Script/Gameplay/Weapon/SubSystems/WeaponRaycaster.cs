using UnityEngine;

public class WeaponRaycaster
{
    private Transform _raycastSource;
    
    public WeaponRaycaster(Transform raycastSource)
    {
        _raycastSource = raycastSource;
    }
    
    public bool TryHitForward(out RaycastHit hit)
    {
        Ray ray = new Ray(_raycastSource.position, _raycastSource.forward);
        if (!Physics.Raycast(ray: ray, out hit)) return false;
        Debug.Log("Hitted object " + hit.collider.gameObject.name);
        return true;
    }
}