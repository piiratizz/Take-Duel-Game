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
    [SerializeField] private PlayerCameraMovement _playerCameraMovement;
    [SerializeField] private PlayerAnimator _playerAnimator;

    private GameplayUIRoot _gameplayUI;
    
    private void Start()
    {
        _gameplayUI = ContainerHolder.Resolve<GameplayUIRoot>();
        
        _playerCameraMovement.Initialize(_playerCameraRoot);
        _playerMovement.Initialize(_playerConfig, _playerAnimator);
        _playerHealth.Initialize(_playerConfig);
        
        AttachCameraToLocalPlayer();
        _playerWeaponInteractor.Initialize();
        
        _damagePerformer.HitEvent.AddListener(ctx =>
        {
            Debug.Log("APPLYING DAMAGE");
            ShowEffectOnTargetPlayer(ctx.Target.connectionToClient);
            _playerHealth.TakeDamage(ctx.BulletDamage);
        });
        
        _playerHealth.DieEvent.AddListener(() =>
        {
            Debug.Log("DIED");
            transform.position = new Vector3(0,1,0);
            _playerHealth.Reset();
        });
        
    }
    
    private void AttachCameraToLocalPlayer()
    {
        _cameraObject.gameObject.SetActive(false);
        _characterIKController.Initialize(_playerCameraRoot.BodyAimTarget);
        
        if (!isLocalPlayer) return;
        
        _cameraObject.gameObject.SetActive(true);
    }

    //Time solution
    [TargetRpc]
    private void ShowEffectOnTargetPlayer(NetworkConnection target)
    {
        _gameplayUI.ScreenHitEffect.PlayEffectAnimation();
    }
}