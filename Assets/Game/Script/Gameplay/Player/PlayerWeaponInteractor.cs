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
    [SerializeField] private PlayerAnimator _playerAnimator;

    [SyncVar] private int _activeWeaponIndex;
    
    private InputSystem_Actions _inputSystem;
    private List<InputAction> _weaponSlots;
    private bool _aimingState = false;
    private List<WeaponController> _weaponControllers;
    
    public void Initialize(PlayerCameraRecoil cameraRecoil)
    {
        _weaponControllers = new List<WeaponController>();
        foreach (var weapon in _weaponList)
        {
            var controller = weapon.GetComponent<WeaponController>();
            controller.Initialize(cameraRecoil, _playerAnimator);
            _weaponControllers.Add(controller);
        }
        
        if (!isLocalPlayer) return;

        AttachWeapon(0);
        
        
        _inputSystem = new InputSystem_Actions();
        _inputSystem.Player.Attack.performed += Shoot;
        _inputSystem.Player.Zoom.performed += ChangeAimingState;
        _inputSystem.Player.PullSlide.performed += PullSlide;
        _inputSystem.Player.Reload.performed += CmdReload;
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
    
    private void CmdReload(InputAction.CallbackContext obj)
    {
        _weaponControllers[_activeWeaponIndex].CmdReload();
    }
    
    private void SelectSlot(InputAction.CallbackContext obj)
    {
        if (obj.action == _weaponSlots[0])
        {
            AttachWeapon(0);
        }
        else if (obj.action == _weaponSlots[1])
        {
            AttachWeapon(1);
        }
        else if (obj.action == _weaponSlots[2])
        {
            AttachWeapon(2);
        }
    }

    private void PullSlide(InputAction.CallbackContext obj)
    {
        _weaponControllers[_activeWeaponIndex].Slide();
    }

    private void Shoot(InputAction.CallbackContext obj)
    {
        _weaponControllers[_activeWeaponIndex].CmdShoot();
    }
    
    private void ChangeAimingState(InputAction.CallbackContext obj)
    {
        if (_aimingState)
        {
            _playerAnimator.PlayAimAnimation();
        }
        else
        {
            _playerAnimator.PlayDeAimAnimation();
        }

        _aimingState = !_aimingState;
    }
    
    private void AttachWeapon(int weaponIndex)
    {
        _weaponControllers[_activeWeaponIndex].UnsubscribeUI();
        _weaponControllers[weaponIndex].SubscribeUI();
        CmdAttachWeapon(weaponIndex);
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
        _playerAnimator.OverrideWeaponHolderAnimator(_weaponList[weaponIndex].AnimatorOverrideController);
        _activeWeaponIndex = weaponIndex;
    }
}