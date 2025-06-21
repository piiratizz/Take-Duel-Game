using Mirror;
using UnityEngine;

public class PlayerAudio : NetworkBehaviour
{
    [SerializeField] private AudioClip[] _stepsSounds;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private PlayerAnimatorEventsHandler _playerAnimatorEventsHandler;
    
    public void Initialize()
    {
        _playerAnimatorEventsHandler.StepEvent.AddListener(CmdPlayStepSound);
    }
    
    [ClientRpc]
    private void RpcPlayStepSound()
    {
        _audioSource.PlayOneShot(_stepsSounds[Random.Range(0,_stepsSounds.Length)]);
    }

    [Command]
    private void CmdPlayStepSound()
    {
        RpcPlayStepSound();
    }
}