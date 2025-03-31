using UnityEngine;

[CreateAssetMenu(menuName = "PlayerConfig", fileName = "PlayerConfig", order = 0)]
public class PlayerConfig : ScriptableObject
{
    public float MovementSpeed;
    public float MovementSpeedChangingModifier;
    public float JumpHeight;
    public float Health;
}