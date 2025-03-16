using System;
using Mirror;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponInteractor : NetworkBehaviour
{
    [SerializeField] private RevolverView _revolver;
    [SerializeField] private RifleView _rifle;
    [SerializeField] private ShotgunView _shotgun;
    
    [SerializeField] private CharacterIKController _ikController;

    private WeaponViewBase _activeWeapon;
    private InputSystem_Actions _inputSystem;
    

    private void Start()
    {
        _revolver.gameObject.SetActive(false);
        _rifle.gameObject.SetActive(false);
        _shotgun.gameObject.SetActive(false);
        
        if(!isLocalPlayer) return;
        
        _inputSystem = new InputSystem_Actions();
        _inputSystem.Player.Attack.performed += Aim;
        _inputSystem.Player.Zoom.performed += DeAim;
        _inputSystem.Player.Slot1.performed += SelectSlot1;
        _inputSystem.Player.Slot2.performed += SelectSlot2;
        _inputSystem.Player.Slot3.performed += SelectSlot3;
        _inputSystem.Enable();
        
        AttachWeapon(_revolver);
    }


    private void SelectSlot1(InputAction.CallbackContext obj)
    {
        AttachWeapon(_revolver);
    }

    private void SelectSlot2(InputAction.CallbackContext obj)
    {
        AttachWeapon(_shotgun);
    }

    private void SelectSlot3(InputAction.CallbackContext obj)
    {
        AttachWeapon(_rifle);
    }
    
    
    private void Aim(InputAction.CallbackContext obj)
    {
        _ikController.SetAimingHandsPosition();
    }
    
    private void DeAim(InputAction.CallbackContext obj)
    {
        _ikController.SetDefaultHandsPosition();
    }
    
    private void AttachWeapon(WeaponViewBase weapon)
    {
        if(_activeWeapon != null)
            _activeWeapon.gameObject.SetActive(false);
        
        weapon.gameObject.SetActive(true);
        
        _activeWeapon = weapon;
        _ikController.AttachHandsToWeapon(_activeWeapon);
    }
}