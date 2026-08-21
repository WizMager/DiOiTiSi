using Ui.Lobby;

namespace Lobby.Services.Ui.Impl
{
    public class UiService : IUiService
    {
        private readonly LobbyWindow _lobbyWindow;
        private readonly MenuWindow _menuWindow;
        private readonly LobbyEnterPopup _lobbyEnterPopup;
        
        public UiService(
            LobbyWindow lobbyWindow, 
            MenuWindow menuWindow, 
            LobbyEnterPopup lobbyEnterPopup
        )
        {
            _lobbyWindow = lobbyWindow;
            _menuWindow = menuWindow;
            _lobbyEnterPopup = lobbyEnterPopup;
        }

        public void OpenWindow(ELobbyWindow window)
        {
            switch (window)
            {
                case ELobbyWindow.LobbyWindow:
                    _lobbyWindow.gameObject.SetActive(true);
                    break;
                case ELobbyWindow.MenuWindow:
                    _menuWindow.gameObject.SetActive(true);
                    break;
                case ELobbyWindow.LobbyEnterPopup:
                    _lobbyEnterPopup.gameObject.SetActive(true);
                    break;
            }
        }

        public void CloseWindow(ELobbyWindow window)
        {
            switch (window)
            {
                case ELobbyWindow.LobbyWindow:
                    _lobbyWindow.gameObject.SetActive(false);
                    break;
                case ELobbyWindow.MenuWindow:
                    _menuWindow.gameObject.SetActive(false);
                    break;
                case ELobbyWindow.LobbyEnterPopup:
                    _lobbyEnterPopup.gameObject.SetActive(false);
                    break;
            }
        }
    }
}