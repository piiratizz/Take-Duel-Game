using System;
using UnityEngine;

public class CameraRoot : MonoBehaviour
{
    [SerializeField] private Transform _bodyAimTarget;
    
    public Transform BodyAimTarget => _bodyAimTarget;
    
}