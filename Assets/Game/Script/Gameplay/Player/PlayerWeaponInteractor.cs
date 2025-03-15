using System;
using Mirror;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponInteractor : NetworkBehaviour
{
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private CharacterIKController _ikController;

    [SerializeField] private RevolverView _revolver;
    
    private InputSystem_Actions _inputSystem;
    

    private void Start()
    {
        if(!isLocalPlayer) return;
        
        _inputSystem = new InputSystem_Actions();
        _inputSystem.Player.Attack.performed += Aim;
        _inputSystem.Player.Zoom.performed += DeAim;
        _inputSystem.Enable();
        
        AttachWeapon(_revolver);
    }

    private void Aim(InputAction.CallbackContext obj)
    {
        _ikController.SetAimingHandsPosition();
    }
    
    private void DeAim(InputAction.CallbackContext obj)
    {
        _ikController.SetDefaultHandsPosition();
    }
    
    public void AttachWeapon(WeaponViewBase weapon)
    {
        _ikController.AttachHandsToWeapon(weapon);
    }
}