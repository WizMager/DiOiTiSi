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
        [SerializeField] private GameObject _playersContainer;
        [SerializeField] private GameObject _playerItemPrefab;
        [SerializeField] private TMP_InputField _roomCode;
        [SerializeField] private Button _readyButton;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private TMP_InputField _changeNicknameField;

        private readonly Dictionary<string, GameObject> _players = new();
        
        private void Start()
        {
            _readyButton.onClick.AddListener(OnReadyClicked);
            _startGameButton.onClick.AddListener(OnStartClicked);
            _roomCode.onSelect.AddListener(OnCodeCLicked);
            _changeNicknameField.onValueChanged.AddListener(OnNicknameChanged);

            _roomCode.text = SessionService.Instance.Session?.Code;
            _startGameButton.gameObject.SetActive(false);

            SessionService.Instance.PlayerJoined += OnPlayerJoined;
            SessionService.Instance.PlayerLeaving += OnPlayerLeaving;
            SessionService.Instance.PlayerPropertiesChanged += OnPlayerPropertyChanged;
        }

        private void OnReadyClicked()
        {
            
        }

        private void OnStartClicked()
        {
            
        }


        private void OnCodeCLicked(string code)
        {
            
        }
        
        private void OnNicknameChanged(string newNickname)
        {
            
        }
        
        private void OnPlayerJoined(string playerId)
        {
            
        }

        private void OnPlayerLeaving(string playerId)
        {
            
        }

        private void OnPlayerPropertyChanged(List<LobbyPlayer> players)
        {
            
        }
    }
}