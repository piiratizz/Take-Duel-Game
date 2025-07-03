using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;

    public void Set(float health)
    {
        _healthSlider.value = health;
    }
}