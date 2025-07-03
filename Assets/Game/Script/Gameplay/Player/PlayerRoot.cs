using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
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
    [Inject] private PlayerAudio _playerAudio;
    [Inject] private GameplayUIRoot _gameplayUI;
    
    [Inject] private NetworkServerStateManager _networkServerStateManager;
    
    public bool Initialized => _initialized;
    private bool _initialized;
    
    public void Initialize()
    {
        _playerCameraMovement.Initialize();
        _playerMovement.Initialize();
        _playerHealth.Initialize();
        _playerUIRoot.Initialize();
        _damagePerformer.Initialize();
        _playerAudio.Initialize();
        
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

        _stateMachine.Initialize(States.Wait);
        
        if (isLocalPlayer)
        {
            Debug.Log(authority);
            CmdMarkPlayerAsReady();
        }
        
        Debug.Log("PLAYER INITIALIZED");
        
        _initialized = true;
    }

    [Command]
    private void CmdMarkPlayerAsReady()
    {
        FindFirstObjectByType<NetworkServerStateManager>().AddReadyPlayer(connectionToClient);
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
        _gameplayUI.HealthBar.Set(_playerHealth.Value);
    }

    [ClientRpc]
    public void ChangeSkin(string skinName)
    {
        Debug.Log($"{_playerMovement} {_playerSkinChanger} {Initialized} {isServer} {netId}");
        _playerSkinChanger.SetSkin(skinName);
    }

    [ClientRpc]
    public void RpcInitializePlayerSteamInfo(string nickname)
    {
        _playerUIRoot.InitializePlayerSteamInfo(nickname);
    }
}