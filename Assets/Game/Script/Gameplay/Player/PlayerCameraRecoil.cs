using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerCameraRecoil : MonoBehaviour
{
    [SerializeField] private PlayerCameraRoot _playerCamera;
    
    public void Perform(WeaponRecoilConfig config)
    {
        var endPoint = _playerCamera.transform.localRotation * Quaternion.Euler(-config.VerticalStrength, config.HorizontalStrength, 0);
        PlayAnimation(config, endPoint);
    }

    private async UniTask PlayAnimation(WeaponRecoilConfig config, Quaternion endPoint)
    {
        float currentTime = config.MoveToNextRecoilPointTime;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            _playerCamera.transform.localRotation = Quaternion.RotateTowards(_playerCamera.transform.localRotation, endPoint,
                config.SpeedModifier * Time.deltaTime);
            await UniTask.Yield();
        }
    }
}