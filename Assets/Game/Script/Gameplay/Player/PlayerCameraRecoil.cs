using UnityEngine;

public class PlayerCameraRecoil : MonoBehaviour
{
    [SerializeField] private PlayerCameraRoot _playerCamera;
    public void Perform(WeaponRecoilConfig config)
    {
        _playerCamera.transform.localRotation *= Quaternion.Euler(-config.VerticalStrength, config.HorizontalStrength, 0);
    }
}