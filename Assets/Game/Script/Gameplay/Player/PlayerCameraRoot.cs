using System;
using UnityEngine;

public class PlayerCameraRoot : MonoBehaviour
{
    [SerializeField] private Transform _raycastPosition;
    [SerializeField] private Transform _bodyAimTarget;
    [SerializeField] private Camera _camera;
    
    public static Camera Camera { get; private set; }
    public static Transform RaycastPosition { get; set; }
    
    public Transform BodyAimTarget => _bodyAimTarget;
    
    private void Awake()
    {
        Camera = _camera;
        RaycastPosition = _raycastPosition;
    }
}