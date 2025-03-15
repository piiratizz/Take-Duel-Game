using System;
using Mirror;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

public class CharacterIKController : NetworkBehaviour
{
    [SerializeField] private Transform _localTarget;
    [SerializeField] private Transform _leftHandTarget;
    [SerializeField] private Transform _rightHandTarget;

    private Transform _aimTarget;
    private bool _initialized;
    
    private WeaponViewBase _weapon;

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
        _leftHandTarget.localPosition = _weapon.HandsConfig.LeftHandLocalPosition;
        _leftHandTarget.localRotation = _weapon.HandsConfig.LeftHandLocalRotation;
        _rightHandTarget.localPosition = _weapon.HandsConfig.RightHandLocalPosition;
        _rightHandTarget.localRotation = _weapon.HandsConfig.RightHandLocalRotation;
    }
    
    public void SetAimingHandsPosition()
    {
        _leftHandTarget.localPosition = _weapon.HandsConfig.LeftHandAimingLocalPosition;
        _leftHandTarget.localRotation = _weapon.HandsConfig.LeftHandAimingLocalRotation;
        _rightHandTarget.localPosition = _weapon.HandsConfig.RightHandAimingLocalPosition;
        _rightHandTarget.localRotation = _weapon.HandsConfig.RightHandAimingLocalRotation;
    }
    

    public void AttachHandsToWeapon(WeaponViewBase weapon)
    {
        _weapon = weapon;
    }
}
