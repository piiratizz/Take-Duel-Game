using Zenject;

public class WaitState : State
{
    private PlayerMovement _playerMovement;
    private PlayerCameraMovement _playerCameraMovement;
    private PlayerWeaponInteractor _playerWeaponInteractor;
    
    public WaitState(DiContainer container, PlayerStateMachine stateMachine) : base(container, stateMachine)
    {
        _playerMovement = container.Resolve<PlayerMovement>();
        _playerCameraMovement = container.Resolve<PlayerCameraMovement>();
        _playerWeaponInteractor = container.Resolve<PlayerWeaponInteractor>();
    }

    public override void Enter()
    {
        _playerMovement.BlockMovement();
        _playerCameraMovement.BlockMovement();
        //_playerWeaponInteractor.BlockWeaponInteraction();
    }

    public override void Exit()
    {
        _playerMovement.UnBlockMovement();
        _playerCameraMovement.UnBlockMovement();
        //_playerWeaponInteractor.UnblockWeaponInteraction();
    }
}