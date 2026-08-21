using System;
using Lobby.Services.Ui;
using Services;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Lobby
{
    public class MenuWindow : MonoBehaviour
    {
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _joinButton;

        private IUiService _uiService;
        
        private void Start()
        {
            _hostButton.onClick.AddListener(OnHostClicked);
            _joinButton.onClick.AddListener(OnJoinClicked);
            SessionService.Instance.OnServicesInitialized += OnServicesInitialized;
        }

        public void Initialize(IUiService uiService)
        {
            _uiService = uiService;
        }

        private async void OnHostClicked()
        {
            try
            {
                _hostButton.interactable = false;
                _joinButton.interactable = false;
                
                var options = new SessionOptions
                {
                    MaxPlayers = 4
                }.WithRelayNetwork();
            
                var session = await MultiplayerService.Instance.CreateSessionAsync(options);
                SessionService.Instance.SetSession(session);
                _uiService.CloseWindow(ELobbyWindow.MenuWindow);
                _uiService.OpenWindow(ELobbyWindow.LobbyWindow);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create session with error {e.Message}");
                _hostButton.interactable = true;
                _joinButton.interactable = true;
            }
        }

        private void OnJoinClicked()
        {
            _uiService.OpenWindow(ELobbyWindow.LobbyEnterPopup);
        }
        
        private void OnServicesInitialized()
        {
            _hostButton.interactable = true;
            _joinButton.interactable = true;
        }

        private void OnDestroy()
        {
            _hostButton.onClick.RemoveAllListeners();
            _joinButton.onClick.RemoveAllListeners();
        }
    }
}