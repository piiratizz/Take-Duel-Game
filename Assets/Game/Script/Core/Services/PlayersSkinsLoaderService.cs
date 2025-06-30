using UnityEngine;
using Zenject;

public class PlayersSkinsLoaderService
{
    [Inject] private PlayerLivesService _playerLivesService;
    [Inject] private CustomNetworkManager _customNetworkManager;
    
    public void SetSkins()
    {
        foreach (var conn in _customNetworkManager.ConnectedPlayer)
        {
            var data = _customNetworkManager.GetPlayerData(conn);
            Debug.Log(conn.identity.GetComponent<PlayerRoot>());
            conn.identity.GetComponent<PlayerRoot>().ChangeSkin(data.SkinName);
        }
    }
}