using Mirror;
using UnityEngine;

public class PlayerAudio : NetworkBehaviour
{
    [SerializeField] private AudioClip[] _stepsSounds;
    [SerializeField] private AudioSource _audioSource;

    [ClientRpc]
    private void RpcPlayStepSound()
    {
        _audioSource.PlayOneShot(_stepsSounds[Random.Range(0,_stepsSounds.Length)]);
    }

    [Command]
    public void CmdPlayStepSound()
    {
        RpcPlayStepSound();
    }
}