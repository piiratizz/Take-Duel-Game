using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LobbyCreator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomNameText;
    [SerializeField] private Button _startMatchButton;
    [SerializeField] private LobbyPlayerView _lobbyPlayerView;
    [SerializeField] private Transform _container;
    [SerializeField] private Texture2D _defaultPlayerAvatar;
    [SerializeField] private WindowsManager _windowsManager;
    
    [Inject] private LobbyService _lobbyService;
    [Inject] private CustomNetworkManager _customNetworkManager;
    [Inject] private SceneService _sceneService;

    private CSteamID _createdLobbyId;
    
    private Dictionary<string, LobbyPlayerView> _playersList;
    
    private void Start()
    {
        _startMatchButton.onClick = new Button.ButtonClickedEvent();
        _startMatchButton.onClick.AddListener(StartGame);
        
        _playersList = new Dictionary<string, LobbyPlayerView>();
        
        _lobbyService.LobbyCreatedEvent.AddListener(OnLobbyCreated);
        _lobbyService.PlayerListUpdatedEvent.AddListener(OnPlayerListUpdated);
        _lobbyService.LobbySingleJoinedEvent.AddListener(OnLobbyJoined);
    }

    private void StartGame()
    {
        _lobbyService.StartGame();
    }
    
    public void InitializeRoomView(LobbyData data)
    {
        _roomNameText.text = data.RoomName;
    }

    public void Create()
    {
        _lobbyService.CreateLobby();
        _windowsManager.ShowLobby();
    }

    public void Join()
    {
        
    }

    private void OnLobbyJoined(LobbyData data)
    {
        _startMatchButton.gameObject.SetActive(false);
        _windowsManager.ShowLobby();
        InitializeRoomView(data);
    }

    private void OnPlayerListUpdated(List<LobbyPlayerData> players)
    {
        UpdatePlayers(players);
    }

    private void OnLobbyCreated(LobbyData data)
    {
        _createdLobbyId = data.LobbyId;
        InitializeRoomView(data);
    }
    
    private void UpdatePlayers(List<LobbyPlayerData> players)
    {
        ClearPlayers();

        foreach (var player in players)
        {
            var instance = Instantiate(_lobbyPlayerView, Vector3.zero, Quaternion.identity, _container);
            instance.Initialize(_defaultPlayerAvatar, player.Nickname);
            _playersList.Add(player.Nickname, instance);
        }
    }

    public void DestroyLobby()
    {
        ClearPlayers();
        _lobbyService.LeaveLobby(_createdLobbyId);
    }

    private void ClearPlayers()
    {
        _playersList.Clear();
        foreach (Transform card in _container)
        {
            Destroy(card.gameObject);
        }
    }
}