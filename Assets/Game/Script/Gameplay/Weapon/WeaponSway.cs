using Mirror;
using UnityEngine;

public class WeaponSway : NetworkBehaviour
{
    [Header("Sway Settings")] 
    [SerializeField] private float _smooth;
    [SerializeField] private float _multiplier;

    private void Update()
    {
        if(!isLocalPlayer) return;
        
        float mouseX = Input.GetAxisRaw("Mouse X") * _multiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _multiplier;
        
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;
        
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, _smooth * Time.deltaTime);
    }
}