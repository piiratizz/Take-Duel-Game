using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerRoot : NetworkBehaviour
{
    [SerializeField] private CharacterIKController _characterIKController;
    [SerializeField] private Camera _cameraObject;
    [FormerlySerializedAs("_cameraRoot")] [SerializeField] private PlayerCameraRoot playerCameraRoot;
    
    private void Start()
    {
        _cameraObject.gameObject.SetActive(false);
        _characterIKController.Initialize(playerCameraRoot.BodyAimTarget);
        
        if (!isLocalPlayer) return;
        
        _cameraObject.gameObject.SetActive(true);
        
    }
}