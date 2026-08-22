using System.Collections.Generic;
using Lobby.Services.Ui;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Lobby
{
    public class LobbyWindow : MonoBehaviour
    {
        [SerializeField] private Transform _playersContainer;
        [SerializeField] private GameObject _playerItemPrefab;
        [SerializeField] private TMP_InputField _roomCode;
        [SerializeField] private Button _readyButton;
        [SerializeField] private Image _readyButtonImage;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private TMP_InputField _changeNicknameField;
        [SerializeField] private Button _leaveLobbyButton;

        private readonly Dictionary<string, PlayerItem> _players = new();
        
        private IUiService _uiService;
        private bool _isReady;
        private string _nickname;
        
        private void Start()
        {
            _readyButton.onClick.AddListener(OnReadyClicked);
            _startGameButton.onClick.AddListener(OnStartClicked);
            _roomCode.onSelect.AddListener(OnCodeClicked);
            _changeNicknameField.onEndEdit.AddListener(OnNicknameChanged);
            _leaveLobbyButton.onClick.AddListener(OnLeaveLobbyClicked);
        }

        public void Initialize(IUiService uiService)
        {
            _uiService = uiService;
        }
        
        private void OnEnable()
        {
            _roomCode.text = SessionService.Instance.Session.Code;
            _startGameButton.gameObject.SetActive(false);
            _isReady = false;
            _nickname = string.Empty;
            _readyButtonImage.color = Color.red;
            _changeNicknameField.text = string.Empty;

            SessionService.Instance.PlayerJoined += OnPlayerJoined;
            SessionService.Instance.PlayerLeaving += OnPlayerLeaving;
            SessionService.Instance.PlayerPropertiesChanged += OnPlayerPropertyChanged;
            SessionService.Instance.OnLeaveLobby += OnLeaveLobby;
            
            OnPlayerPropertyChanged(SessionService.Instance.GetLobbyPlayers());
        }

        private void OnReadyClicked()
        {
            _isReady = !_isReady;
            _readyButtonImage.color = _isReady ? Color.green : Color.red;
            SessionService.Instance.SetReady(_isReady);
        }

        private void OnStartClicked()
        {
            _startGameButton.interactable = false;
            SessionService.Instance.StartGame();
        }

        private void OnCodeClicked(string code)
        {
            code = code.Trim();
            GUIUtility.systemCopyBuffer = code;
        }
        
        private void OnNicknameChanged(string newNickname)
        {
            if (_nickname == newNickname)
                return;
            
            _nickname = newNickname;
            SessionService.Instance.SetNickname(newNickname);
        }
        
        private void OnLeaveLobbyClicked()
        {
            _uiService.CloseWindow(ELobbyWindow.LobbyWindow);
            SessionService.Instance.LeaveLobby();
            _uiService.OpenWindow(ELobbyWindow.MenuWindow);
        }
        
        private void OnPlayerJoined()
        {
            OnPlayerPropertyChanged(SessionService.Instance.GetLobbyPlayers());
        }

        private void OnPlayerLeaving(string playerId)
        {
            if (_players.Remove(playerId, out var item))
            {
                Destroy(item.gameObject);
            }
            
            OnPlayerPropertyChanged(SessionService.Instance.GetLobbyPlayers());
        }

        private void OnPlayerPropertyChanged(List<LobbyPlayer> players)
        {
            var readyCounter = 0;
            foreach (var player in players)
            {
                if (_players.ContainsKey(player.Id))
                {
                    _players[player.Id].SetNickname(player.Nickname);
                    _players[player.Id].SetReadyStatus(player.IsReady);
                }
                else
                {
                    CreatePlayerItem(player.Id, player.Nickname, player.IsReady);
                }

                if (player.IsReady)
                {
                    readyCounter++;
                }
            }

            _startGameButton.gameObject.SetActive(SessionService.Instance.Session.IsHost && readyCounter == players.Count);
        }

        private void OnLeaveLobby()
        {
            _uiService.CloseWindow(ELobbyWindow.LobbyWindow);
            _uiService.OpenWindow(ELobbyWindow.MenuWindow);
        }
        
        private void CreatePlayerItem(string playerId, string nickname, bool isReady)
        {
            var playerItem = Instantiate(_playerItemPrefab, _playersContainer).GetComponent<PlayerItem>();
            playerItem.SetNickname(nickname);
            playerItem.SetReadyStatus(isReady);
            _players.Add(playerId, playerItem);
        }

        private void OnDisable()
        {
            SessionService.Instance.PlayerJoined -= OnPlayerJoined;
            SessionService.Instance.PlayerLeaving -= OnPlayerLeaving;
            SessionService.Instance.PlayerPropertiesChanged -= OnPlayerPropertyChanged;
            SessionService.Instance.OnLeaveLobby -= OnLeaveLobby;

            foreach (var (_, playerItem) in _players)
            {
                Destroy(playerItem.gameObject);
            }
            _players.Clear();
        }

        private void OnDestroy()
        {
            SessionService.Instance.PlayerJoined -= OnPlayerJoined;
            SessionService.Instance.PlayerLeaving -= OnPlayerLeaving;
            SessionService.Instance.PlayerPropertiesChanged -= OnPlayerPropertyChanged;
            SessionService.Instance.OnLeaveLobby -= OnLeaveLobby;
        }
    }
}