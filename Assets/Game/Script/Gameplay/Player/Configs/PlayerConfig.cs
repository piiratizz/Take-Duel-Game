using UnityEngine;

[CreateAssetMenu(menuName = "PlayerConfig", fileName = "PlayerConfig", order = 0)]
public class PlayerConfig : ScriptableObject
{
    public float MovementSpeed;
    public float Health;
}