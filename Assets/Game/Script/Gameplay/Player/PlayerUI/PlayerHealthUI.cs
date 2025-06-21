using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : NetworkBehaviour
{
    [SerializeField] private Slider _hpSlider;

    public void UpdateHealth(float newHealth)
    {
        _hpSlider.value = newHealth;
    }
}
