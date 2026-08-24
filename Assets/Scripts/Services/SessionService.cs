using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services
{
    public class SessionService : MonoBehaviour
    {
        public event Action PlayerJoined;
        public event Action<string> PlayerLeaving;
        public event Action<List<LobbyPlayer>> PlayerPropertiesChanged;
        public event Action OnServicesInitialized;
        public event Action OnLeaveLobby;

        [SerializeField] private bool _isDebugStart;
        
        private bool _isInitialized;
        
        public static SessionService Instance { get; private set; }
        
        public ISession Session { get; private set; }

        public bool IsSignedIn => UnityServices.State == ServicesInitializationState.Initialized &&
                                  AuthenticationService.Instance.IsSignedIn;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning($"Multiple {nameof(SessionService)} instances have been destroyed!");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            InitializeServices();
        }

        public void SetSession(ISession session)
        {
            if (session == null || _isInitialized)
                return;
            
            _isInitialized = true;
            Session = session;

            Session.PlayerJoined += OnPlayerJoined;
            Session.PlayerHasLeft += OnPlayerLeaving;
            Session.PlayerPropertiesChanged += OnPlayerPropertyChanged;
            Session.SessionPropertiesChanged += OnSessionPropertyChanged;
            Session.SessionHostChanged += OnSessionHostChanged;
            Session.RemovedFromSession += OnRemovedFromSession;
        }

        private void OnPlayerJoined(string playerId)
        {
            PlayerJoined?.Invoke();
        }

        private void OnPlayerLeaving(string playerId)
        {
            PlayerLeaving?.Invoke(playerId);
        }

        private void OnPlayerPropertyChanged()
        {
            PlayerPropertiesChanged?.Invoke(GetLobbyPlayers());
        }

        public List<LobbyPlayer> GetLobbyPlayers()
        {
            var playersInfoList = new List<LobbyPlayer>();
            foreach (var player in Session.Players)
            {
                player.Properties.TryGetValue("nick", out var nickname);
                player.Properties.TryGetValue("ready", out var isReady);

                playersInfoList.Add(new LobbyPlayer
                {
                    Id = player.Id,
                    Nickname = nickname?.Value ?? $"Player{player.Id}",
                    IsReady = isReady?.Value == "1"
                });
            }
            
            return playersInfoList;
        }

        private void OnSessionPropertyChanged()
        {
            if (Session.Properties.TryGetValue("state", out var state) && state.Value == "starting")
            {
                SceneManager.LoadScene("Game");
            }
        }
        
        private void OnSessionHostChanged(string _)
        {
            OnPlayerPropertyChanged();
        }
        
        private void OnRemovedFromSession()
        {
            OnLeaveLobby?.Invoke();
            ClearSession();
        }
        
        public async void SetNickname(string nickname)
        {
            try
            {
                Session.CurrentPlayer.SetProperty("nick", new PlayerProperty(nickname, VisibilityPropertyOptions.Member));
                await Session.SaveCurrentPlayerDataAsync();
            
                OnPlayerPropertyChanged();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to set nickname with error {e.Message}");
            }
        }

        public async void SetReady(bool isReady)
        {
            try
            {
                Session.CurrentPlayer.SetProperty("ready", new PlayerProperty(isReady ? "1" : "0", VisibilityPropertyOptions.Member));
                await Session.SaveCurrentPlayerDataAsync();
            
                OnPlayerPropertyChanged();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to set player ready with error {e.Message}");
            }
        }

        public async Task StartGame()
        {
            Session.AsHost().SetProperty("state", new SessionProperty("starting", VisibilityPropertyOptions.Member));
            await Session.AsHost().SavePropertiesAsync();
            SceneManager.LoadScene("Game");
        }

        public async void LeaveLobby()
        {
            try
            {
                await Session.LeaveAsync();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error leaving lobby with error {e.Message}");
            }
            finally
            {
                ClearSession();
            }
        }
        
        private async void InitializeServices()
        {
            try
            {
                if (UnityServices.State != ServicesInitializationState.Initialized)
                    await UnityServices.InitializeAsync();

                if (!AuthenticationService.Instance.IsSignedIn)
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (_isDebugStart)
                {
                    var session = await MultiplayerService.Instance.CreateOrJoinSessionAsync("dioitisi-debug", new SessionOptions
                    {
                        MaxPlayers = 2,
                        Name = "debug"
                    }.WithRelayNetwork());
                    
                    SetSession(session);
                    SceneManager.LoadScene("Game");
                    return;
                }
                
                OnServicesInitialized?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to initialize services with error {e.Message}");
            }
        }
        
        private void ClearSession()
        {
            if (Session == null)
                return;
            
            Session.PlayerJoined -= OnPlayerJoined;
            Session.PlayerHasLeft -= OnPlayerLeaving;
            Session.PlayerPropertiesChanged -= OnPlayerPropertyChanged;
            Session.SessionPropertiesChanged -= OnSessionPropertyChanged;
            Session.SessionHostChanged -= OnSessionHostChanged;
            Session.RemovedFromSession -= OnRemovedFromSession;
            
            _isInitialized = false;
            Session = null;
        }
        
        private void OnDestroy()
        {
            if (_isDebugStart)
            {
                CloseSessionOnTeardown();
            }
            else
            {
                if (Session != null)
                {
                    Session.PlayerJoined -= OnPlayerJoined;
                    Session.PlayerHasLeft -= OnPlayerLeaving;
                    Session.PlayerPropertiesChanged -= OnPlayerPropertyChanged;
                    Session.SessionPropertiesChanged -= OnSessionPropertyChanged;
                    Session.SessionHostChanged -= OnSessionHostChanged;
                    Session.RemovedFromSession -= OnRemovedFromSession;
                }
            }
            

            if (Instance == this)
            {
                Instance = null;
            }
        }

        private async void CloseSessionOnTeardown()
        {
            try
            {
                var session = Session;
                
                if (session == null)
                    return;
                
                ClearSession();
                
                if (session.IsHost)
                {
                    await session.AsHost().DeleteAsync();
                }
                else
                {
                    await session.LeaveAsync();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}