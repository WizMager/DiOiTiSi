namespace Lobby.Services.Ui
{
    public interface IUiService
    {
        void OpenWindow(ELobbyWindow window);
        void CloseWindow(ELobbyWindow window);
    }
}