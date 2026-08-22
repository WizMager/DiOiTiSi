using System.Collections.Generic;
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

        private readonly Dictionary<string, PlayerItem> _players = new();
        
        private bool _isReady;
        private string _nickname;
        
        private void Start()
        {
            _readyButton.onClick.AddListener(OnReadyClicked);
            _startGameButton.onClick.AddListener(OnStartClicked);
            _roomCode.onSelect.AddListener(OnCodeClicked);
            _changeNicknameField.onEndEdit.AddListener(OnNicknameChanged);
        }

        private void OnEnable()
        {
            _roomCode.text = SessionService.Instance.Session.Code;
            _startGameButton.gameObject.SetActive(false);

            SessionService.Instance.PlayerJoined += OnPlayerJoined;
            SessionService.Instance.PlayerLeaving += OnPlayerLeaving;
            SessionService.Instance.PlayerPropertiesChanged += OnPlayerPropertyChanged;
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
        
        private void OnPlayerJoined(string playerId)
        {
            CreatePlayerItem(playerId, $"Player{playerId}", false);
        }

        private void OnPlayerLeaving(string playerId)
        {
            var playerItem = _players[playerId];
            _players.Remove(playerId);
            Destroy(playerItem);
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

            _startGameButton.gameObject.SetActive(readyCounter == players.Count);
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
        }

        private void OnDestroy()
        {
            SessionService.Instance.PlayerJoined -= OnPlayerJoined;
            SessionService.Instance.PlayerLeaving -= OnPlayerLeaving;
            SessionService.Instance.PlayerPropertiesChanged -= OnPlayerPropertyChanged;
        }
    }
}