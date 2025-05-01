using Mirror;
using UnityEngine;
using Zenject;

public class PlayerStateService : NetworkBehaviour
{
    [Inject] private ServerPlayersService _serverPlayersService;
    
    [Server]
    public void SetAllPlayersState(States state)
    {
        foreach (var player in Object.FindObjectsByType<PlayerRoot>(FindObjectsSortMode.None))
            player.SetState(state);

        RpcSetAllPlayersState(state);
    }

    [ClientRpc]
    private void RpcSetAllPlayersState(States state)
    {
        foreach (var player in Object.FindObjectsByType<PlayerRoot>(FindObjectsSortMode.None))
            player.SetState(state);
    }

    [Server]
    public void SetPlayerState(uint netId, States state)
    {
        RpcSetPlayerState(netId, state);
    }

    [ClientRpc]
    private void RpcSetPlayerState(uint netId, States state)
    {
        var player = _serverPlayersService.GetPlayerById(netId);
        if (player != null)
        {
            player.GetComponent<PlayerRoot>()?.SetState(state);
        }
    }
}