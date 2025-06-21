using UnityEngine;
using Zenject;

public class LobbyItemsManager : MonoBehaviour
{
    [SerializeField] private LobbyItemView _lobbyItemView;
    [SerializeField] private Transform _container;

    [Inject] private SteamManager _steamManager;
    [Inject] private LobbyService _lobbyService;
    
    public void FindLobbies()
    {
        _steamManager.RequestLobbiesList(Create);
    }
    
    private void Create(LobbyData lobbyData)
    {
        if(lobbyData.RoomName == string.Empty) return;
        
        var instance = Instantiate(_lobbyItemView, Vector3.zero, Quaternion.identity, _container);
        instance.Initialize(lobbyData);
        instance.JoinClicked.AddListener(OnLobbyJoinClicked);
    }

    public void ClearLobbies()
    {
        foreach (Transform lobby in _container)
        {
            Destroy(lobby.gameObject);
        }
    }
    
    private void OnLobbyJoinClicked(LobbyData data)
    {
        
    }
}