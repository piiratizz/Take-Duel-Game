using System.Threading;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    [SerializeField] private Animator _animator;
    
    private static readonly int XDirection = Animator.StringToHash("XDirection");
    private static readonly int YDirection = Animator.StringToHash("YDirection");
    private static readonly int Jump = Animator.StringToHash("Jump");
    
    public void PlayWalkingAnimation(float xDirection, float yDirection)
    {
        _animator.SetFloat(XDirection, xDirection);
        _animator.SetFloat(YDirection, yDirection);
    }

    public void PlayJumpAnimation()
    {
        _animator.SetBool(Jump, true);
        RestoreJumpStateAfterDelay();
    }

    private async UniTask RestoreJumpStateAfterDelay()
    {
        await UniTask.WaitForEndOfFrame();
        await UniTask.WaitForEndOfFrame();
        await UniTask.WaitForEndOfFrame();
        
        _animator.SetBool(Jump, false);
    }
}