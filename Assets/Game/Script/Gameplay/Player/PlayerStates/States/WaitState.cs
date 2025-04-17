using Zenject;

public class WaitState : State
{
    private PlayerMovement _playerMovement;
    private PlayerCameraMovement _playerCameraMovement;
    
    public WaitState(DiContainer container, PlayerStateMachine stateMachine) : base(container, stateMachine)
    {
        _playerMovement = container.Resolve<PlayerMovement>();
        _playerCameraMovement = container.Resolve<PlayerCameraMovement>();
    }

    public override void Enter()
    {
        _playerMovement.BlockMovement();
        _playerCameraMovement.BlockMovement();
    }

    public override void Exit()
    {
        
    }
}