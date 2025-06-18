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

    private bool _skipFrame;
    
    public void Initialize(Transform aimTarget)
    {
        _aimTarget = aimTarget;
        _initialized = true;
    }

    private void UpdateAimTargetPosition()
    {
        _localTarget.position = _aimTarget.position;
    }
    
    private async void Update()
    {
        if (!_initialized) return;

        UpdateAimTargetPosition();

        if (_skipFrame)
        {
            await UniTask.Yield();
            _skipFrame = false;
        }
        
        if (_weapon != null)
        {
            UpdateHandsPosition();
        }
    }

    private void UpdateHandsPosition()
    {
        _leftHandTarget.position = _weapon.LeftHandPosition.position;
        _leftHandTarget.rotation = _weapon.LeftHandPosition.rotation;
        
        _rightHandTarget.position = _weapon.RightHandPosition.position;
        _rightHandTarget.rotation = _weapon.RightHandPosition.rotation;
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
        _skipFrame = true;
        _weapon = weapon;
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
