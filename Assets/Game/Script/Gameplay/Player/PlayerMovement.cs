using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private Transform _groundCheckPosition;
    [SerializeField] private LayerMask _groundCheckLayers;
    
    private float _maxSpeed;
    private float _speedModifier;
    private float _jumpHeight;
    
    private InputSystem_Actions _playerInput;
    private CharacterController _characterController;
    private PlayerAnimator _playerAnimator;
    private const float Gravity = 9.81f;
    private bool _initialized = false;
    private Vector3 _velocity;
    
    private float MinVelocityY = -10f;
    private float MaxVelocityY = 10f;
    
    public void Initialize(PlayerConfig config, PlayerAnimator animator)
    {
        if (!isLocalPlayer) return;
        
        _maxSpeed = config.MovementSpeed;
        _speedModifier = config.MovementSpeedChangingModifier;
        _jumpHeight = config.JumpHeight;
        
        _playerInput = new InputSystem_Actions();
        _playerInput.Player.Move.Enable();
        
        _playerInput.Player.Jump.performed += TryJump;
        _playerInput.Player.Jump.Enable();
        
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
        
        _velocity.x = Mathf.Lerp(_velocity.x, _maxSpeed * direction.x, _speedModifier);
        _velocity.z = Mathf.Lerp(_velocity.z, _maxSpeed * direction.z, _speedModifier);
        _velocity.y -= Gravity * Time.deltaTime;
        _velocity.y = Mathf.Clamp(_velocity.y, MinVelocityY, MaxVelocityY);
        
        _playerAnimator.PlayWalkingAnimation(_velocity.x, _velocity.z);
        MovePlayer();
    }

    private void MovePlayer()
    {
        _characterController.Move((transform.right * _velocity.x + transform.forward * _velocity.z) * Time.deltaTime);
        _characterController.Move(Vector3.up * _velocity.y * Time.deltaTime);
    }
    
    private void TryJump(InputAction.CallbackContext obj)
    {
        if(!IsGrounded()) return;
        
        _playerAnimator.PlayJumpAnimation();
        _velocity.y = _jumpHeight;
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(_groundCheckPosition.position, 0.1f, _groundCheckLayers);
    }
}
