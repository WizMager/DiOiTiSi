using Ui.Lobby;

namespace Lobby.Services.Ui.Impl
{
    public class UiService : IUiService
    {
        private readonly LobbyWindow _lobbyWindow;
        private readonly MenuWindow _menuWindow;
        
        public UiService(
            LobbyWindow lobbyWindow, 
            MenuWindow menuWindow
        )
        {
            _lobbyWindow = lobbyWindow;
            _menuWindow = menuWindow;
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
                case ELobbyWindow.EnterLobbyCodePopup:
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
                case ELobbyWindow.EnterLobbyCodePopup:
                    break;
            }
        }
    }
}