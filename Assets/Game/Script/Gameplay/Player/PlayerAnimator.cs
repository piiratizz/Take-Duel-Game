using System.Threading;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimator : NetworkBehaviour
{
    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private Animator _weaponHolderAnimator;
    
    private static readonly int XDirection = Animator.StringToHash("XDirection");
    private static readonly int YDirection = Animator.StringToHash("YDirection");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Aiming = Animator.StringToHash("Aiming");
    private static readonly int Shoot = Animator.StringToHash("Shoot");

    public void PlayWalkingAnimation(float xDirection, float yDirection)
    {
        _characterAnimator.SetFloat(XDirection, xDirection);
        _characterAnimator.SetFloat(YDirection, yDirection);
    }

    public void PlayJumpAnimation()
    {
        _characterAnimator.SetBool(Jump, true);
        RestoreJumpStateAfterDelay();
    }

    private async UniTask RestoreJumpStateAfterDelay()
    {
        await UniTask.Yield();
        await UniTask.Yield();
        await UniTask.Yield();
        
        _characterAnimator.SetBool(Jump, false);
    }

    public void PlayAimAnimation()
    {
       _weaponHolderAnimator.SetBool(Aiming, true);
    }
    
    public void PlayDeAimAnimation()
    {
        _weaponHolderAnimator.SetBool(Aiming, false);
    }

    public void PlayShootAnimation()
    {
        _weaponHolderAnimator.SetTrigger(Shoot);
    }

    public void OverrideWeaponHolderAnimator(AnimatorOverrideController overrideController)
    {
        _weaponHolderAnimator.runtimeAnimatorController = overrideController;
    }
}