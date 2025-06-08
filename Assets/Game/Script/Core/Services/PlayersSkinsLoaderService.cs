using UnityEngine;
using Zenject;

public class PlayersSkinsLoaderService
{
    [Inject] private ServerPlayersService _serverPlayersService;
    [Inject] private CustomNetworkManager _customNetworkManager;
    
    public void SetSkins()
    {
        foreach (var conn in _serverPlayersService.PlayersConnections)
        {
            var data = _customNetworkManager.GetPlayerData(conn);
            Debug.Log(data.SkinName);
            conn.identity.GetComponent<PlayerRoot>().ChangeSkin(data.SkinName);
        }
    }
}