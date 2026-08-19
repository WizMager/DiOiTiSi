using Lobby.Services.Ui;
using Lobby.Services.Ui.Impl;
using Ui;
using UnityEngine;

namespace Lobby
{
    public class LobbyBootstrap : MonoBehaviour
    {
        [SerializeField] private LobbyWindow _lobbyWindow;
        [SerializeField] private MenuWindow _menuWindow;

        private void Start()
        {
            IUiService uiService = new UiService(_lobbyWindow, _menuWindow);
            
            _menuWindow.Initialize(uiService);
        }
    }
}