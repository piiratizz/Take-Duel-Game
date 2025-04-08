using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCameraRecoil : MonoBehaviour
{
    [SerializeField] private Transform _playerShakeCameraContainer;
    [SerializeField] private Transform _playerRootCameraContainer;
    [Header("Shake configuration")]
    [SerializeField] private bool _shakeEffect;
    [SerializeField] private float _shakeDuration;
    [SerializeField] private Vector3 _shakeSrength;
    [SerializeField] private float _randomness = 90;
    [SerializeField] private int _vibrato = 10;
    [SerializeField] private ShakeRandomnessMode _randomnessMode = ShakeRandomnessMode.Full;
    [SerializeField] private bool _fadeOut = true;
    
    public void Perform(WeaponRecoilConfig config)
    {
        var endPoint = _playerRootCameraContainer.transform.localRotation * Quaternion.Euler(-config.VerticalStrength, config.HorizontalStrength, 0);
        
        if (_shakeEffect)
        {
            DoShake();
        }
            
        PlayAnimation(config, endPoint);
    }

    private async UniTask PlayAnimation(WeaponRecoilConfig config, Quaternion endPoint)
    {
        float currentTime = config.MoveToNextRecoilPointTime;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            _playerRootCameraContainer.transform.localRotation = Quaternion.RotateTowards(_playerRootCameraContainer.transform.localRotation, endPoint,
                config.SpeedModifier * Time.deltaTime);
            await UniTask.Yield();
        }
    }

    private void DoShake()
    {
        _playerShakeCameraContainer.transform.DOShakeRotation(
            _shakeDuration,
            _shakeSrength,
            _vibrato,
            _randomness,
            _fadeOut,
            _randomnessMode
        );
    }
}