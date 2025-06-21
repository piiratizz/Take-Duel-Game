using Cysharp.Threading.Tasks;
using Steamworks;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class LobbyListWindow : WindowBase
{
    [SerializeField] private LobbyItemsManager _lobbyItemsManager;

    public override void Open()
    {
        base.Open();
        _lobbyItemsManager.FindLobbies();
    }

    public override void Close()
    {
        base.Close();
        _lobbyItemsManager.ClearLobbies();
    }
}