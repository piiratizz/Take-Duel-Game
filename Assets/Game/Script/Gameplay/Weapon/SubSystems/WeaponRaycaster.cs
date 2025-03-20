using UnityEngine;

public class WeaponRaycaster
{
    private Camera _camera;
    
    public WeaponRaycaster(Camera camera)
    {
        _camera = camera;
    }
    
    public bool TryHit(out RaycastHit hit)
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit);
    }
}