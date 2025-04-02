using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

public class CharacterIKController : NetworkBehaviour
{
    [SerializeField] private Transform _localTarget;
    [SerializeField] private Transform _leftHandTarget;
    [SerializeField] private Transform _rightHandTarget;
    [SerializeField] private float _handsAttachSpeed = 0.3f;
    [SerializeField] private Rig _handsRig;
    
    private Transform _aimTarget;
    private bool _initialized;
    
    private WeaponViewBase _weapon;

    private CancellationTokenSource _cancellationToken;
    
    public void Initialize(Transform aimTarget)
    {
        _aimTarget = aimTarget;
        _initialized = true;
    }

    private void UpdateAimTargetPosition()
    {
        _localTarget.position = _aimTarget.position;
    }
    
    private void Update()
    {
        if (!_initialized) return;

        UpdateAimTargetPosition();
    }

    public void SetDefaultHandsPosition()
    {
        PlayHandsAnimation(
            _leftHandTarget,
            _rightHandTarget,
            _weapon.HandsConfig.LeftHandLocalPosition,
            _weapon.HandsConfig.RightHandLocalPosition);
        
        //_leftHandTarget.localPosition = _weapon.HandsConfig.LeftHandLocalPosition;
        _leftHandTarget.localRotation = _weapon.HandsConfig.LeftHandLocalRotation;
        //_rightHandTarget.localPosition = _weapon.HandsConfig.RightHandLocalPosition;
        _rightHandTarget.localRotation = _weapon.HandsConfig.RightHandLocalRotation;
    }
    
    public void SetAimingHandsPosition()
    {
        PlayHandsAnimation(
            _leftHandTarget,
            _rightHandTarget,
            _weapon.HandsConfig.LeftHandAimingLocalPosition,
            _weapon.HandsConfig.RightHandAimingLocalPosition);
        
        //_leftHandTarget.localPosition = _weapon.HandsConfig.LeftHandAimingLocalPosition;
        _leftHandTarget.localRotation = _weapon.HandsConfig.LeftHandAimingLocalRotation;
        //_rightHandTarget.localPosition = _weapon.HandsConfig.RightHandAimingLocalPosition;
        _rightHandTarget.localRotation = _weapon.HandsConfig.RightHandAimingLocalRotation;
    }

    private async void PlayHandsAnimation(Transform left, Transform right, Vector3 leftPosition, Vector3 rightPosition)
    {
        _cancellationToken?.Cancel();
        await UniTask.WaitForEndOfFrame();
        _cancellationToken = new CancellationTokenSource();
        
        while (left.localPosition != leftPosition
               && right.localPosition != rightPosition)
        {
            if (_cancellationToken.IsCancellationRequested) break;
            
            left.localPosition = Vector3.Lerp(
                left.localPosition,
                leftPosition,
                _handsAttachSpeed * Time.deltaTime);
        
            right.localPosition = Vector3.Lerp(
                right.localPosition,
                rightPosition,
                _handsAttachSpeed * Time.deltaTime);

            await UniTask.Yield();
        }
        
    }

    public void AttachHandsToWeapon(WeaponViewBase weapon)
    {
        _weapon = weapon;
        
        // Time solution
        SetDefaultHandsPosition();
    }

    public void DisableHandsIK()
    {
        _handsRig.weight = 0;
    }
    
    public void EnableHandsIK()
    {
        _handsRig.weight = 1;
    }
}
