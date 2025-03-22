using Mirror;
using UnityEngine;

public class PlayerRoot : NetworkBehaviour
{
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private CharacterIKController _characterIKController;
    [SerializeField] private Camera _cameraObject;
    [SerializeField] private PlayerCameraRoot _playerCameraRoot;
    [SerializeField] private PlayerWeaponInteractor _playerWeaponInteractor;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerDamagePerformer _damagePerformer;
    
    private void Start()
    {
        _playerMovement.Initialize(_playerConfig);
        _playerHealth.Initialize(_playerConfig);
        
        AttachCameraToLocalPlayer();
        
        //_damagePerformer.HitEvent.AddListener(ctx => _playerHealth.TakeDamage(ctx.BulletDamage));
        _damagePerformer.HitEvent.AddListener(ctx => Debug.Log("DAMAGED"));

        _playerWeaponInteractor.Initialize();
    }
    

    private void AttachCameraToLocalPlayer()
    {
        _cameraObject.gameObject.SetActive(false);
        _characterIKController.Initialize(_playerCameraRoot.BodyAimTarget);
        
        if (!isLocalPlayer) return;
        
        _cameraObject.gameObject.SetActive(true);
    }
}