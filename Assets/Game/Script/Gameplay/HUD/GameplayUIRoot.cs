using UnityEngine;

public class GameplayUIRoot : MonoBehaviour
{
    [SerializeField] private WeaponHudUI _weaponHud;
    [SerializeField] private ScreenHitEffectUI _screenHitEffect;
    [SerializeField] private RoundTimerUI _roundTimer;
    [SerializeField] private LivesCountUI _livesCounter;
    [SerializeField] private HealthBarUI _healthBar;
    
    public WeaponHudUI WeaponHud => _weaponHud;
    public ScreenHitEffectUI ScreenHitEffect=> _screenHitEffect;
    public RoundTimerUI RoundTimer => _roundTimer;
    public LivesCountUI LivesCounter => _livesCounter;
    public HealthBarUI HealthBar => _healthBar;
}