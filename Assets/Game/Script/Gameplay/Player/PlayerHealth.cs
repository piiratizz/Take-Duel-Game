using Mirror;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class PlayerHealth : NetworkBehaviour
{
    [HideInInspector] public UnityEvent DieEvent;

    [Inject] private PlayerConfig _config;
    private float _startHealth;
    
    [SyncVar] private float _currentHealth;

    public float Value => _currentHealth;
    
    public void Initialize()
    {
        _startHealth = _config.Health;
        _currentHealth = _startHealth;
        DieEvent = new UnityEvent();
    }
    
    public void TakeDamage(float damage)
    {
        Debug.Log("Player hit");
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            RpcInvokeDieEvent();
        }
    }

    public void Reset()
    {
        _currentHealth = _startHealth;
    }
    
    
    [ClientRpc]
    private void RpcInvokeDieEvent()
    {
        Debug.Log("DIED RPC");
        DieEvent.Invoke();
    }
}