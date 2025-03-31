using UnityEngine;

public class GameplayUIRoot : MonoBehaviour
{
    [SerializeField] private WeaponHudUI _weaponHud;
    [SerializeField] private ScreenHitEffectUI _screenHitEffect;
    
    public WeaponHudUI WeaponHud => _weaponHud;
    public ScreenHitEffectUI ScreenHitEffect=> _screenHitEffect;
}