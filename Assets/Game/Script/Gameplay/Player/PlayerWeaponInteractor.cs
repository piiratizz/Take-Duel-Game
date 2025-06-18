using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mirror;
using NaughtyAttributes;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerWeaponInteractor : NetworkBehaviour
{
    [SerializeField] private List<WeaponViewBase> _weaponList;
    
    [Inject] private WeaponHolderEventsHandler _weaponHolderEventsHandler;
    [Inject] private CharacterIKController _ikController;
    [Inject] private PlayerAnimator _playerAnimator;
    
    [SyncVar] private int _activeWeaponIndex;
    
    private InputSystem_Actions _inputSystem;
    private List<InputAction> _weaponSlots;
    private bool _aimingState = false;
    private List<WeaponController> _weaponControllers;
    private bool _switchingWeapon = false;
    private bool _weaponChangeMoment = false;
    
    
    public void Initialize()
    {
        _weaponControllers = new List<WeaponController>();
        foreach (var weapon in _weaponList)
        {
            var controller = weapon.GetComponent<WeaponController>();
            controller.Initialize();
            _weaponControllers.Add(controller);
        }
        
        AttachWeapon(0);
        
        if (!isLocalPlayer) return;
        
        _weaponHolderEventsHandler.SwitchWeaponStartedEvent.AddListener(OnSwitchWeaponStarted);
        _weaponHolderEventsHandler.SwitchWeaponEndedEvent.AddListener(OnSwitchWeaponEnded);
        _weaponHolderEventsHandler.SwitchWeaponChangeMomentEvent.AddListener(OnWeaponChangeMoment);
        
        _inputSystem = new InputSystem_Actions();

        BindInput();
        
        _inputSystem.Enable();
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
        _weaponControllers[_activeWeaponIndex].Shoot();
    }
    
    private void Reload(InputAction.CallbackContext obj)
    {
        _weaponControllers[_activeWeaponIndex].Reload();
    }
    
    private void ChangeAimingState(InputAction.CallbackContext obj)
    {
        _aimingState = !_aimingState;
        
        if (_aimingState)
        {
            _playerAnimator.PlayAimAnimation();
        }
        else
        {
            _playerAnimator.PlayDeAimAnimation();
        }
    }
    
    private async void AttachWeapon(int weaponIndex)
    {
        if(!_weaponControllers[_activeWeaponIndex].Reloaded) return;
        if(_switchingWeapon) return;
        
        _playerAnimator.PlaySwitchAnimation();
        
        await UniTask.WaitUntil(() => _weaponChangeMoment == true);
        _weaponChangeMoment = false;
        
        _weaponControllers[_activeWeaponIndex].UnsubscribeUI();
        _weaponHolderEventsHandler.ReloadStartedEvent.RemoveListener(_weaponControllers[_activeWeaponIndex].OnReloadStarted);
        _weaponHolderEventsHandler.ReloadCompleteEvent.RemoveListener(_weaponControllers[_activeWeaponIndex].OnReloadComplete);
        
        _weaponControllers[weaponIndex].SubscribeUI();
        _weaponHolderEventsHandler.ReloadStartedEvent.AddListener(_weaponControllers[weaponIndex].OnReloadStarted);
        _weaponHolderEventsHandler.ReloadCompleteEvent.AddListener(_weaponControllers[weaponIndex].OnReloadComplete);
    
        CmdAttachWeapon(weaponIndex);
    }

    private void OnSwitchWeaponStarted() => _switchingWeapon = true;
    private void OnSwitchWeaponEnded() => _switchingWeapon = false;
    private void OnWeaponChangeMoment() => _weaponChangeMoment = true;
    
    [Command]
    private void CmdAttachWeapon(int weaponIndex)
    {
        RpcUpdateWeapon(weaponIndex);
    }
    
    [ClientRpc]
    private async void RpcUpdateWeapon(int weaponIndex)
    {
        for (int i = 0; i < _weaponList.Count; i++)
        {
            if (i == weaponIndex)
            {
                await UniTask.Yield();
                _weaponList[i].gameObject.SetActive(true);
                
            }
            else
            {
                _weaponList[i].gameObject.SetActive(false);
            }
        }
        
        _ikController.AttachHandsToWeapon(_weaponList[weaponIndex]);
        _playerAnimator.OverrideWeaponHolderAnimator(_weaponList[weaponIndex].AnimatorOverrideController);
        _activeWeaponIndex = weaponIndex;
    }

    public void BlockWeaponInteraction()
    {
        if(!isLocalPlayer) return;
        
        _inputSystem.Player.Attack.Disable();
        _inputSystem.Player.Zoom.Disable();
        _inputSystem.Player.PullSlide.Disable();
        _inputSystem.Player.Reload.Disable();
        _inputSystem.Player.PullSlide.Disable();
    }

    public void UnblockWeaponInteraction()
    {
        if(!isLocalPlayer) return;
        
        _inputSystem.Player.Attack.Enable();
        _inputSystem.Player.Zoom.Enable();
        _inputSystem.Player.PullSlide.Enable();
        _inputSystem.Player.Reload.Enable();
        _inputSystem.Player.PullSlide.Enable();
    }

    private void BindInput()
    {
        _inputSystem.Player.Attack.performed += Shoot;
        _inputSystem.Player.Zoom.performed += ChangeAimingState;
        _inputSystem.Player.PullSlide.performed += PullSlide;
        _inputSystem.Player.Reload.performed += Reload;
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
    }

    private void OnDestroy()
    {
        if(!isLocalPlayer) return;
        
        _inputSystem.Dispose();
    }
}