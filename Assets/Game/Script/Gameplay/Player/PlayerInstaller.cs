using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using Zenject;

public class PlayerInstaller : NetworkBehaviour
{
    [SerializeField] private PlayerRoot _playerRoot;
    [SerializeField] private PlayerUIRoot _playerUIRoot;
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private CharacterIKController _characterIKController;
    [SerializeField] private Camera _cameraObject;
    [SerializeField] private PlayerCameraRoot _playerCameraRoot;
    [SerializeField] private PlayerWeaponInteractor _playerWeaponInteractor;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerDamagePerformer _damagePerformer;
    [SerializeField] private PlayerCameraMovement _playerCameraMovement;
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private PlayerCameraRecoil _playerCameraRecoil;
    [SerializeField] private LagCompensator _lagCompensator;
    [SerializeField] private WeaponHolderEventsHandler _weaponHolderEventsHandler;
    [SerializeField] private PlayerStateMachine _playerStateMachine;
    [SerializeField] private PlayerRagdollController _playerRagdollController;
    [SerializeField] private PlayerSkinChanger _playerSkinChanger;
    [SerializeField] private PlayerAudio _playerAudio;
    
    
    private DiContainer _container;
    
    private bool _installed = false;
    
    public override async void OnStartClient()
    {
        if(_installed) return;
        _installed = true;
        
        var networkServerStateManager = FindFirstObjectByType<NetworkServerStateManager>();
        await UniTask.WaitWhile(() => networkServerStateManager.IsReady == false);

        
        _container = new DiContainer(ContainerHolder.Instance);
        InstallBindings();
        InjectAll();
        _playerRoot.Initialize();
    }
    
    private void InstallBindings()
    {
        _container.Bind<PlayerRoot>().FromInstance(_playerRoot).AsSingle();
        _container.Bind<PlayerUIRoot>().FromInstance(_playerUIRoot).AsSingle();
        _container.Bind<PlayerConfig>().FromInstance(_playerConfig).AsSingle();
        _container.Bind<CharacterIKController>().FromInstance(_characterIKController).AsSingle();
        _container.Bind<Camera>().FromInstance(_cameraObject).AsSingle();
        _container.Bind<PlayerCameraRoot>().FromInstance(_playerCameraRoot).AsSingle();
        _container.Bind<PlayerWeaponInteractor>().FromInstance(_playerWeaponInteractor).AsSingle();
        _container.Bind<PlayerHealth>().FromInstance(_playerHealth).AsSingle();
        _container.Bind<PlayerMovement>().FromInstance(_playerMovement).AsSingle();
        _container.Bind<PlayerDamagePerformer>().FromInstance(_damagePerformer).AsSingle();
        _container.Bind<PlayerCameraMovement>().FromInstance(_playerCameraMovement).AsSingle();
        _container.Bind<PlayerAnimator>().FromInstance(_playerAnimator).AsSingle();
        _container.Bind<PlayerCameraRecoil>().FromInstance(_playerCameraRecoil).AsSingle();
        _container.Bind<LagCompensator>().FromInstance(_lagCompensator).AsSingle();
        _container.Bind<WeaponHolderEventsHandler>().FromInstance(_weaponHolderEventsHandler).AsSingle();
        _container.Bind<PlayerStateMachine>().FromInstance(_playerStateMachine).AsSingle();
        _container.Bind<PlayerRagdollController>().FromInstance(_playerRagdollController).AsSingle();
        _container.Bind<PlayerSkinChanger>().FromInstance(_playerSkinChanger).AsSingle();
        _container.Bind<PlayerAudio>().FromInstance(_playerAudio).AsSingle();
        
        Debug.Log("PLAYER INSTALLED");
    }

    private void InjectAll()
    {
        _container.InjectGameObject(_playerRoot.gameObject);
    }
}