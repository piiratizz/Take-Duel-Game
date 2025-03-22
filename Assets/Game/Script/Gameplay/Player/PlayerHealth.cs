using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : NetworkBehaviour
{
    [HideInInspector] public UnityEvent DieEvent;
    private float _startHealth;
    [SyncVar] private float _currentHealth;

    public void Initialize(PlayerConfig config)
    {
        _startHealth = config.Health;
        _currentHealth = _startHealth;
        DieEvent = new UnityEvent();
    }
    
    [Command]
    public void CmdTakeDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _startHealth);

        if (_currentHealth == 0)
        {
            DieEvent.Invoke();
        }
    }
}