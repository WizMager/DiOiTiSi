using System;
using Lobby.Services.Ui;
using Services;
using TMPro;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Lobby
{
    public class LobbyEnterPopup : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _roomCodeField;
        [SerializeField] private Button _applyCodeButton;
        [SerializeField] private Button _closePopupButton;

        private IUiService _uiService;
        private bool _isConnecting;
        
        private void Start()
        {
            _roomCodeField.onEndEdit.AddListener(OnEnterCode);
            _applyCodeButton.onClick.AddListener(() => OnEnterCode(_roomCodeField.text));
            _closePopupButton.onClick.AddListener(OnPopupClosed);
        }

        public void Initialize(IUiService uiService)
        {
            _uiService = uiService;
        }
        
        private void OnEnterCode(string code)
        {
            if (!_isConnecting)
                return;
            
            if(string.IsNullOrEmpty(code))
                return;
            
            _isConnecting = true;
            code = code.Trim();
            
            ConnectToLobby(code);
        }

        private async void ConnectToLobby(string code)
        {
            try
            {
                _applyCodeButton.interactable = false;
                _closePopupButton.interactable = false;
                _roomCodeField.interactable = false;
                var session = await MultiplayerService.Instance.JoinSessionByCodeAsync(code);
                SessionService.Instance.SetSession(session);
                _uiService.CloseWindow(ELobbyWindow.MenuWindow);
                _uiService.CloseWindow(ELobbyWindow.LobbyEnterPopup);
                _uiService.OpenWindow(ELobbyWindow.LobbyWindow);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to join session with error {e.Message}");
                _applyCodeButton.interactable = true;
                _closePopupButton.interactable = true;
                _roomCodeField.interactable = true;
                _isConnecting = false;
            }
        }
        
        private void OnPopupClosed()
        {
            _uiService.CloseWindow(ELobbyWindow.LobbyEnterPopup);
        }
        
        private void OnDestroy()
        {
            _roomCodeField.onEndEdit.RemoveAllListeners();
            _applyCodeButton.onClick.RemoveAllListeners();
            _closePopupButton.onClick.RemoveAllListeners();
        }
    }
}