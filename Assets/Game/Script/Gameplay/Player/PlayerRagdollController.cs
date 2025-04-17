using UnityEngine;
using Zenject;

public class PlayerRagdollController : MonoBehaviour
{
    [SerializeField] private GameObject _defaultPlayerObject;
    [SerializeField] private GameObject _ragdollObject;

    [SerializeField] private Transform _defaultCameraMovementTarget;
    [SerializeField] private Transform _ragdollCameraMovementTarget;
    
    [Inject] private PlayerCameraMovement _playerCameraMovement;
    
    public void ActivateRagdoll()
    {
        _playerCameraMovement.SetCameraMovementTarget(_ragdollCameraMovementTarget);
        _playerCameraMovement.EnableForceLookingOnTarget(_ragdollCameraMovementTarget);
        _defaultPlayerObject.gameObject.SetActive(false);
        _ragdollObject.gameObject.SetActive(true);
    }

    public void DeactivateRagdoll()
    {
        _playerCameraMovement.SetCameraMovementTarget(_defaultCameraMovementTarget);
        _playerCameraMovement.DisableForceLookingOnTarget();
        _defaultPlayerObject.gameObject.SetActive(true);
        _ragdollObject.gameObject.SetActive(false);
    }
}
