using System;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;

public class EffectDestroyer : NetworkBehaviour
{
    [SerializeField] private float _timeToDestroy = 5f;

    private async void Start()
    {
        await DestroyAfterDelay();
    }

    private async UniTask DestroyAfterDelay()
    {
        await UniTask.WaitForSeconds(_timeToDestroy);
        if (isServer)
        {
            NetworkServer.Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
