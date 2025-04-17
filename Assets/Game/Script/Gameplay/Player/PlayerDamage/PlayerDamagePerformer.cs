using Mirror;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class PlayerDamagePerformer : NetworkBehaviour, IDamageable
{
    [HideInInspector] public UnityEvent<HitContext> HitEvent = new UnityEvent<HitContext>();
    [Inject] private PlayerHealth _playerHealth;
    [Inject] private PlayerUIRoot _playerUIRoot;
    [Inject] private GameplayUIRoot _gameplayUI;
    [Inject] private PlayerStateMachine _playerStateMachine;
    
    public void Initialize()
    {
        _playerHealth.DieEvent.AddListener(() =>
        {
            _playerStateMachine.SetState(States.Dead);
        });
    }

    [Server]
    public void PerformHit(HitContext ctx)
    {
        HitEvent.Invoke(ctx);
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