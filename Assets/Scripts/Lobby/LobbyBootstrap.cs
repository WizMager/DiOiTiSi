using Lobby.Services.Ui;
using Lobby.Services.Ui.Impl;
using Ui.Lobby;
using UnityEngine;

namespace Lobby
{
    public class LobbyBootstrap : MonoBehaviour
    {
        [SerializeField] private LobbyWindow _lobbyWindow;
        [SerializeField] private MenuWindow _menuWindow;
        [SerializeField] private LobbyEnterPopup _lobbyEnterPopup;

        private void Start()
        {
            IUiService uiService = new UiService(_lobbyWindow, _menuWindow, _lobbyEnterPopup);
            
            _menuWindow.Initialize(uiService);
            _lobbyEnterPopup.Initialize(uiService);
            _lobbyWindow.Initialize(uiService);
        }
    }
}