using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float _speed;
    
    private InputSystem_Actions _playerInput;
    private CharacterController _characterController;
    
    private void Start()
    {
        _playerInput = new InputSystem_Actions();
        _playerInput.Player.Move.Enable();

        _characterController = GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        if (!isLocalPlayer) return;

        var playerInputDirection = _playerInput.Player.Move.ReadValue<Vector2>();
        var direction = new Vector3(playerInputDirection.x, 0, playerInputDirection.y);
        
        MovePlayer(direction);
    }

    private void MovePlayer(Vector3 direction)
    {
        _characterController.Move((direction.x * transform.right + direction.z * transform.forward) * _speed * Time.deltaTime);
    }
}
