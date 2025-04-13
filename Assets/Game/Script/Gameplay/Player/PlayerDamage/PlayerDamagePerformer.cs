using Mirror;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class PlayerDamagePerformer : NetworkBehaviour, IDamageable
{
    [Inject] private PlayerHealth _playerHealth;
    [Inject] private PlayerUIRoot _playerUIRoot;
    [Inject] private GameplayUIRoot _gameplayUI;
    
    public void Initialize()
    {
        _playerHealth.DieEvent.AddListener(() =>
        {
            Debug.Log($"DIED {netId}");
            transform.position = new Vector3(0,1,0);
            _playerHealth.Reset();
            _playerUIRoot.CmdUpdateHealth(_playerHealth.Value);
        });
    }
    
    [Server]
    public void PerformHit(HitContext ctx)
    {
        Debug.Log("APPLYING DAMAGE");
        ShowEffectOnTargetPlayer(ctx.Target.connectionToClient);
        _playerHealth.TakeDamage(ctx.BulletDamage);
        _playerUIRoot.RpcUpdateHealth(_playerHealth.Value);
    }

    [TargetRpc]
    private void ShowEffectOnTargetPlayer(NetworkConnection target)
    {
        _gameplayUI.ScreenHitEffect.PlayEffectAnimation();
    }
    
    public NetworkIdentity GetNetworkIdentity()
    {
        return netIdentity;
    }
}