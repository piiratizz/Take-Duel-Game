using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : NetworkBehaviour
{
    private float _maxSpeed;
    private float _currentSpeedX;
    private float _currentSpeedY;
    private float _speedModifier;
    
    private InputSystem_Actions _playerInput;
    private CharacterController _characterController;
    private PlayerAnimator _playerAnimator;
    private const float Gravity = 9.81f;
    private bool _initialized = false;

    public void Initialize(PlayerConfig config, PlayerAnimator animator)
    {
        if (!isLocalPlayer) return;
        
        _maxSpeed = config.MovementSpeed;
        _speedModifier = config.MovementSpeedChangingModifier;
        _currentSpeedX = 0;
        
        _playerInput = new InputSystem_Actions();
        _playerInput.Player.Move.Enable();
        
        _playerAnimator = animator;
        _characterController = GetComponent<CharacterController>();
        _initialized = true;
    }
    
    private void Update()
    {
        if(!_initialized) return;
        if (!isLocalPlayer) return;
        var playerInputDirection = _playerInput.Player.Move.ReadValue<Vector2>();
        var direction = new Vector3(playerInputDirection.x, 0, playerInputDirection.y);
        

        _currentSpeedX = Mathf.Lerp(_currentSpeedX, _maxSpeed * direction.x, _speedModifier);
        _currentSpeedY = Mathf.Lerp(_currentSpeedY, _maxSpeed * direction.z, _speedModifier);
       
        _playerAnimator.PlayWalkingAnimation(_currentSpeedX, _currentSpeedY);
        MovePlayer();
    }

    private void MovePlayer()
    {
        _characterController.Move((transform.right * _currentSpeedX + transform.forward * _currentSpeedY) * Time.deltaTime);
        _characterController.Move(Vector3.down * Gravity * Time.deltaTime);
    }
}
