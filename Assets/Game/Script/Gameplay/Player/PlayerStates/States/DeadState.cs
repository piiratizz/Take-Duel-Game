using UnityEngine;
using Zenject;

public class DeadState : State
{
    private PlayerHealth _playerHealth;
    private PlayerUIRoot _playerUIRoot;
    private PlayerRoot _playerRoot;
    private PlayerRagdollController _playerRagdollController;
    private PlayerMovement _playerMovement;
    private PlayerCameraMovement _playerCameraMovement;
    
    public DeadState(DiContainer container, PlayerStateMachine stateMachine) : base(container, stateMachine)
    {
        _playerHealth = container.Resolve<PlayerHealth>();
        _playerUIRoot = container.Resolve<PlayerUIRoot>();
        _playerRoot = container.Resolve<PlayerRoot>();
        _playerRagdollController = container.Resolve<PlayerRagdollController>();
        _playerMovement = container.Resolve<PlayerMovement>();
        _playerCameraMovement = container.Resolve<PlayerCameraMovement>();
    }
    
    public override void Enter()
    {
        Debug.Log($"DIED {_playerRoot.netId}");
        _playerMovement.BlockMovement();
        _playerCameraMovement.BlockMovement();
        
        //_playerRagdollController.ActivateRagdoll();
        //StateMachine.ClientRpcCall();
    }

    public override void OnClientRPCCall()
    {
        if (_playerRagdollController.IsRagdollActive)
        {
            _playerRagdollController.DeactivateRagdoll();
        }
        else
        {
            _playerRagdollController.ActivateRagdoll();
        }
    }
    
    public override void Exit()
    {
        _playerMovement.UnBlockMovement();
        _playerCameraMovement.UnBlockMovement();
        
        //_playerRagdollController.DeactivateRagdoll();
        //StateMachine.ClientRpcCall();
        
        _playerHealth.Reset();
        _playerUIRoot.CmdUpdateHealth(_playerHealth.Value);
    }


}