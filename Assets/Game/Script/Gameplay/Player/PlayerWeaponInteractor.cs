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
    
    private int _activeWeaponIndex;
    private InputSystem_Actions _inputSystem;
    private List<InputAction> _weaponSlots;

    public void Initialize()
    {
        CmdAttachWeapon(0);
        if (!isLocalPlayer) return;

        _inputSystem = new InputSystem_Actions();
        _inputSystem.Player.Attack.performed += Aim;
        _inputSystem.Player.Zoom.performed += DeAim;

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
    
    private void Aim(InputAction.CallbackContext obj)
    {
        _ikController.SetAimingHandsPosition();
    }

    private void DeAim(InputAction.CallbackContext obj)
    {
        _ikController.SetDefaultHandsPosition();
    }

    [Command]
    private void CmdAttachWeapon(int weaponIndex)
    {
        RpcUpdateWeapon(weaponIndex);
    }
    
    [ClientRpc]
    private void RpcUpdateWeapon(int weaponIndex)
    {
        _ikController.AttachHandsToWeapon(_weaponList[weaponIndex]);
        _activeWeaponIndex = weaponIndex;
        for (int i = 0; i < _weaponList.Count; i++)
        {
            _weaponList[i].gameObject.SetActive(i == weaponIndex);
        }
    }
}