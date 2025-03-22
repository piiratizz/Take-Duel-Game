using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : NetworkBehaviour
{
    private float _speed;
    private InputSystem_Actions _playerInput;
    private CharacterController _characterController;
    private const float Gravity = 9.81f;
    private bool _initialized = false;

    public void Initialize(PlayerConfig config)
    {
        if (!isLocalPlayer) return;
        _speed = config.MovementSpeed;
        _playerInput = new InputSystem_Actions();
        _playerInput.Player.Move.Enable();

        _characterController = GetComponent<CharacterController>();
        _initialized = true;
    }
    
    private void Update()
    {
        if(!_initialized) return;
        if (!isLocalPlayer) return;
        var playerInputDirection = _playerInput.Player.Move.ReadValue<Vector2>();
        var direction = new Vector3(playerInputDirection.x, 0, playerInputDirection.y);
        
        MovePlayer(direction);
    }

    private void MovePlayer(Vector3 direction)
    {
        _characterController.Move((direction.x * transform.right + direction.z * transform.forward) * _speed * Time.deltaTime);
        _characterController.Move(Vector3.down * Gravity * Time.deltaTime);
    }
}
