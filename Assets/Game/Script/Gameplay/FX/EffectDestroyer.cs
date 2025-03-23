using System;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;

public class EffectDestroyer : MonoBehaviour
{
    [SerializeField] private float _timeToDestroy = 5f;

    private async void Start()
    {
       await DestroyAfterDelay();
    }

    private async UniTask DestroyAfterDelay()
    {
        await UniTask.WaitForSeconds(_timeToDestroy);
        NetworkServer.Destroy(gameObject);
        Destroy(gameObject);
    }
}
