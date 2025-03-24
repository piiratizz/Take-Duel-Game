using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private static readonly int XDirection = Animator.StringToHash("XDirection");
    private static readonly int YDirection = Animator.StringToHash("YDirection");

    public void PlayWalkingAnimation(float xDirection, float yDirection)
    {
        _animator.SetFloat(XDirection, xDirection);
        _animator.SetFloat(YDirection, yDirection);
    }
}