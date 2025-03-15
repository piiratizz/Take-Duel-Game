using System;
using UnityEngine;

public class PlayerCameraRoot : MonoBehaviour
{
    [SerializeField] private Transform _bodyAimTarget;
    
    public Transform BodyAimTarget => _bodyAimTarget;
    
}