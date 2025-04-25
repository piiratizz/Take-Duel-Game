using Mirror;
using UnityEngine;
using Zenject;

public class PlayerRagdollController : NetworkBehaviour
{
    [SerializeField] private GameObject _defaultPlayerObject;
    [SerializeField] private GameObject _ragdollObject;

    [SerializeField] private Transform _defaultCameraMovementTarget;
    [SerializeField] private Transform _ragdollCameraMovementTarget;
    
    [Inject] private PlayerCameraMovement _playerCameraMovement;

    public bool IsRagdollActive { get; private set; }
    
    public void ActivateRagdoll()
    {
        _playerCameraMovement.SetCameraMovementTarget(_ragdollCameraMovementTarget);
        _playerCameraMovement.EnableForceLookingOnTarget(_ragdollCameraMovementTarget);
        _defaultPlayerObject.gameObject.SetActive(false);
        _ragdollObject.gameObject.SetActive(true);
        IsRagdollActive = true;
    }

    public void DeactivateRagdoll()
    {
        _playerCameraMovement.SetCameraMovementTarget(_defaultCameraMovementTarget);
        _playerCameraMovement.DisableForceLookingOnTarget();
        _defaultPlayerObject.gameObject.SetActive(true);
        _ragdollObject.gameObject.SetActive(false);
        IsRagdollActive = false;
    }
}
