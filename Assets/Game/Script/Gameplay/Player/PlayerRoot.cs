using Mirror;
using UnityEngine;
using Zenject;

public class PlayerRoot : NetworkBehaviour
{
    [Inject] private PlayerUIRoot _playerUIRoot;
    [Inject] private CharacterIKController _characterIKController;
    [Inject] private Camera _cameraObject;
    [Inject] private PlayerCameraRoot _playerCameraRoot;
    [Inject] private PlayerWeaponInteractor _playerWeaponInteractor;
    [Inject] private PlayerHealth _playerHealth;
    [Inject] private PlayerMovement _playerMovement;
    [Inject] private PlayerDamagePerformer _damagePerformer;
    [Inject] private PlayerCameraMovement _playerCameraMovement;
    [Inject] private PlayerModelChanger _playerModelChanger;
    
    private GameplayUIRoot _gameplayUI;
    
    private void Start()
    {
        _gameplayUI = ContainerHolder.Resolve<GameplayUIRoot>();
        _playerCameraMovement.Initialize();
        _playerMovement.Initialize();
        _playerHealth.Initialize();
        _playerUIRoot.Initialize();
        _damagePerformer.Initialize();
        
        AttachCameraToLocalPlayer();
        _playerWeaponInteractor.Initialize();
        
        
        if (isLocalPlayer)
        {
            _playerModelChanger.SetLocalModel();
        }
        else
        {
            _playerModelChanger.SetGlobalModel();
        }
        
    }
    
    private void AttachCameraToLocalPlayer()
    {
        _cameraObject.gameObject.SetActive(false);
        _characterIKController.Initialize(_playerCameraRoot.BodyAimTarget);
        
        if (!isLocalPlayer) return;
        
        _cameraObject.gameObject.SetActive(true);
    }

    //Time solution

}