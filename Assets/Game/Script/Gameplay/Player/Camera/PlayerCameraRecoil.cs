using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class PlayerCameraRecoil : MonoBehaviour
{
    [SerializeField] private Transform _playerShakeCameraContainer;
    [SerializeField] private Transform _playerRecoilCameraContainer;
    [Header("Shake configuration")]
    [SerializeField] private bool _shakeEffect;
    [SerializeField] private float _shakeDuration;
    [SerializeField] private Vector3 _shakeSrength;
    [SerializeField] private float _randomness = 90;
    [SerializeField] private int _vibrato = 10;
    [SerializeField] private ShakeRandomnessMode _randomnessMode = ShakeRandomnessMode.Full;
    [SerializeField] private bool _fadeOut = true;

    [Inject] private PlayerCameraMovement _playerCameraMovement;
    
    public void Perform(WeaponRecoilConfig config)
    {
        var endPoint = _playerCameraMovement.VerticalRotation - config.VerticalStrength;
        
        if (_shakeEffect)
        {
            DoShake();
        }
            
        PlayAnimation(config, endPoint);
    }

    private async void PlayAnimation(WeaponRecoilConfig config, float endPoint)
    {
        float currentTime = config.MoveToNextRecoilPointTime;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            var verticalEulerRotation = Mathf.Lerp(
                _playerCameraMovement.VerticalRotation,
                endPoint,
                config.SpeedModifier * Time.deltaTime);
            
            _playerCameraMovement.SetVerticalRotation(verticalEulerRotation);

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