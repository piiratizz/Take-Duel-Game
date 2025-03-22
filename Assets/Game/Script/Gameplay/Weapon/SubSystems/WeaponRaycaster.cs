using UnityEngine;

public class WeaponRaycaster
{
    private Camera _camera;
    
    public WeaponRaycaster(Camera camera)
    {
        _camera = camera;
    }
    
    public bool TryHit(out IHitPerformer hitPerformer)
    {
        hitPerformer = null;
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var isHit = Physics.Raycast(ray, out var hit);
        
        if (!isHit) return false;

        return hit.collider.TryGetComponent(out hitPerformer);
    }
}