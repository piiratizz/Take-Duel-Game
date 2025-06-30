using UnityEngine;

public class LobbyCreationWindow : WindowBase
{
    [SerializeField] private LobbyCreator _lobbyCreator;
    
    public override void Close()
    {
        base.Close();
        _lobbyCreator.DestroyLobby();
    }
}