using System;
using System.Collections.Generic;
using Mirror;
using NaughtyAttributes;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponInteractor : NetworkBehaviour
{
    [SerializeField] private List<WeaponViewBase> _weaponList;
    [SerializeField] private CharacterIKController _ikController;

    [SyncVar] private int _activeWeaponIndex;
    
    private InputSystem_Actions _inputSystem;
    private List<InputAction> _weaponSlots;
    private bool _aimingState = false;
    private List<WeaponController> _weaponControllers;

    public void Initialize()
    {
        _weaponControllers = new List<WeaponController>();
        foreach (var weapon in _weaponList)
        {
            _weaponControllers.Add(weapon.GetComponent<WeaponController>());
        }
        
        if (!isLocalPlayer) return;

        CmdAttachWeapon(0);
        
        _inputSystem = new InputSystem_Actions();
        _inputSystem.Player.Attack.performed += Shoot;
        _inputSystem.Player.Zoom.performed += ChangeAimingState;
        _inputSystem.Player.PullSlide.performed += PullSlide;

        _weaponSlots = new List<InputAction>()
        {
            _inputSystem.Player.Slot1,
            _inputSystem.Player.Slot2,
            _inputSystem.Player.Slot3
        };

        foreach (var slot in _weaponSlots)
        {
            slot.performed += SelectSlot;
        }

        _inputSystem.Enable();
    }


    private void SelectSlot(InputAction.CallbackContext obj)
    {
        if (obj.action == _weaponSlots[0])
        {
            CmdAttachWeapon(0);
        }
        else if (obj.action == _weaponSlots[1])
        {
            CmdAttachWeapon(1);
        }
        else if (obj.action == _weaponSlots[2])
        {
            CmdAttachWeapon(2);
        }
    }

    private void PullSlide(InputAction.CallbackContext obj)
    {

    }

    private void Shoot(InputAction.CallbackContext obj)
    {
        CmdShoot();
    }

    [Command]
    private void CmdShoot()
    {
        _weaponControllers[_activeWeaponIndex].Shoot();
    }
    
    private void ChangeAimingState(InputAction.CallbackContext obj)
    {
        if (_aimingState)
        {
            _ikController.SetAimingHandsPosition();
        }
        else
        {
            _ikController.SetDefaultHandsPosition();
        }

        _aimingState = !_aimingState;
    }

    [Command]
    private void CmdAttachWeapon(int weaponIndex)
    {
        _activeWeaponIndex = weaponIndex;
        RpcUpdateWeapon(weaponIndex);
    }
    
    [ClientRpc]
    private void RpcUpdateWeapon(int weaponIndex)
    {
        for (int i = 0; i < _weaponList.Count; i++)
        {
            _weaponList[i].gameObject.SetActive(i == weaponIndex);
        }
        
        _ikController.AttachHandsToWeapon(_weaponList[weaponIndex]);
        _activeWeaponIndex = weaponIndex;
    }
}