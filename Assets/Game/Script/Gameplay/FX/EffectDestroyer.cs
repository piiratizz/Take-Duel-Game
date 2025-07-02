using System;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;

public class EffectDestroyer : NetworkBehaviour
{
    [SerializeField] private float _timeToDestroy = 5f;

    private async void Start()
    {
        if(!isServer) return;
        
        await DestroyAfterDelay();
    }

    private async UniTask DestroyAfterDelay()
    {
        await UniTask.WaitForSeconds(_timeToDestroy);
        NetworkServer.Destroy(gameObject);
        Destroy(gameObject);
    }
}
