using UnityEngine;
using Zenject;

public class DeadState : State
{
    private PlayerRoot _playerRoot;
    private PlayerRagdollController _playerRagdollController;
    private PlayerMovement _playerMovement;
    private PlayerCameraMovement _playerCameraMovement;
    
    public DeadState(DiContainer container, PlayerStateMachine stateMachine) : base(container, stateMachine)
    {
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
    }


}