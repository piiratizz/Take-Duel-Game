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
    [Inject] private PlayerStateMachine _stateMachine;
    [Inject] private PlayerSkinChanger _playerSkinChanger;

    public bool Initialized => _initialized;
    private bool _initialized;
    
    private void Start()
    {
        _playerCameraMovement.Initialize();
        _playerMovement.Initialize();
        _playerHealth.Initialize();
        _playerUIRoot.Initialize();
        _damagePerformer.Initialize();
        _stateMachine.Initialize(States.Fight);
        
        AttachCameraToLocalPlayer();
        _playerWeaponInteractor.Initialize();
        
        if (isLocalPlayer)
        {
            _playerSkinChanger.SetNoHeadMesh();
        }
        else
        {
            _playerSkinChanger.SetHeadMesh();
        }

        _initialized = true;
    }
    
    private void AttachCameraToLocalPlayer()
    {
        _cameraObject.gameObject.SetActive(false);
        _characterIKController.Initialize(_playerCameraRoot.BodyAimTarget);
        
        if (!isLocalPlayer) return;
        
        _cameraObject.gameObject.SetActive(true);
    }

    public void SetState(States state)
    {
        _stateMachine.SetState(state);
    }

    public void StopMove()
    {
        _playerMovement.StopPlayer();
    }

    [ClientRpc]
    public void RpcTeleportTo(Vector3 position)
    {
        transform.position = position;
    }

    [ClientRpc]
    public void RpcRotate(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    [Server]
    public void ResetHealthFromServer()
    {
        _playerHealth.Reset();
        _playerUIRoot.RpcUpdateHealth(_playerHealth.Value);
    }

    [ClientRpc]
    public void ChangeSkin(string skinName)
    {
        _playerSkinChanger.SetSkin(skinName);
    }
}