using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : NetworkBehaviour
{
    [HideInInspector] public UnityEvent DieEvent;
    private float _startHealth;
    [SyncVar] private float _currentHealth;

    public float Value => _currentHealth;
    
    public void Initialize(PlayerConfig config)
    {
        _startHealth = config.Health;
        _currentHealth = _startHealth;
        DieEvent = new UnityEvent();
    }
    
    public void TakeDamage(float damage)
    {
        Debug.Log("Player hit");
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            CmdInvokeDieEvent();
        }
    }

    public void Reset()
    {
        _currentHealth = _startHealth;
    }

    [Command]
    private void CmdInvokeDieEvent()
    {
        DieEvent.Invoke();
        RpcInvokeDieEvent();
    }
    
    [ClientRpc]
    private void RpcInvokeDieEvent()
    {
        if(netIdentity.isServer) return;
        DieEvent.Invoke();
    }
}