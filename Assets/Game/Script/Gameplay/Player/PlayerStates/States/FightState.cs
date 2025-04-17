using Zenject;

public class FightState : State
{
    private PlayerMovement _playerMovement;
    private PlayerCameraMovement _playerCameraMovement;
    
    public FightState(DiContainer container, PlayerStateMachine stateMachine) : base(container, stateMachine)
    {
        _playerMovement = container.Resolve<PlayerMovement>();
        _playerCameraMovement = container.Resolve<PlayerCameraMovement>();
    }

    public override void Enter()
    {
        _playerMovement.UnBlockMovement();
        _playerCameraMovement.UnBlockMovement();
    }

    public override void Exit()
    {
        
    }
}