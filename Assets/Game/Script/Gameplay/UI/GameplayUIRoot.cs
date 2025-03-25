using UnityEngine;

public class GameplayUIRoot : MonoBehaviour
{
    [SerializeField] private WeaponHudUI _weaponHud;
    
    public WeaponHudUI WeaponHud => _weaponHud;
}