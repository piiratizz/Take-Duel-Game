using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ScreenHitEffectUI : MonoBehaviour
{
    [SerializeField][Range(0,1)] private float _tranparentChangingModifier;
    [SerializeField] private RawImage _screenEffect;

    private CancellationTokenSource _cancellationToken;
    private Color _startEffectColor;
    
    private void Start()
    {
        _startEffectColor = _screenEffect.color;
        _screenEffect.gameObject.SetActive(false);
    }

    public void PlayEffectAnimation()
    {
        _cancellationToken?.Cancel();
        _cancellationToken = new CancellationTokenSource();
        _screenEffect.color = _startEffectColor;
        ShowEffectAsync();
    }

    private async UniTask ShowEffectAsync()
    {
        Debug.Log("STARTING ANIMATION");
        _screenEffect.gameObject.SetActive(true);
        while (_screenEffect.color.a > 0)
        {
            if (_cancellationToken.IsCancellationRequested) break;
            
            _screenEffect.color = new Color(
                _startEffectColor.r,
                _startEffectColor.g,
                _startEffectColor.b,
                _screenEffect.color.a - _tranparentChangingModifier * Time.deltaTime
                );
            await UniTask.Yield();
        }
        _screenEffect.gameObject.SetActive(false);
    }
}
