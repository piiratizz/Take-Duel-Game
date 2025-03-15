using UnityEngine;

[CreateAssetMenu(menuName = "HandConfigs/HandConfig", fileName = "HandConfig", order = 0)]
public class HandsIKConfig : ScriptableObject
{
    public Vector3 LeftHandLocalPosition;
    public Quaternion LeftHandLocalRotation;
    
    public Vector3 RightHandLocalPosition;
    public Quaternion RightHandLocalRotation;

    public Vector3 RightHandAimingLocalPosition;
    public Quaternion RightHandAimingLocalRotation;
    
    public Vector3 LeftHandAimingLocalPosition;
    public Quaternion LeftHandAimingLocalRotation;
}