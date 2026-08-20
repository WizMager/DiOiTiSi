using System;
using System.Collections.Generic;
using Unity.Services.Multiplayer;
using UnityEngine;

namespace Services
{
    public class SessionService : MonoBehaviour
    {
        public event Action<string> PlayerJoined;
        public event Action<string> PlayerLeaving;
        public event Action<List<LobbyPlayer>> PlayerPropertiesChanged;
        
        private bool _isInitialized;
        
        public static SessionService Instance { get; private set; }

        public ISession Session { get; private set; }
        
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

        public void SetSession(ISession session)
        {
            if (session == null || _isInitialized)
                return;
            
            _isInitialized = true;
            Session = session;

            Session.PlayerJoined += OnPlayerJoined;
            Session.PlayerLeaving += OnPlayerLeaving;
            Session.PlayerPropertiesChanged += OnPlayerPropertyChanged;
        }

        private void OnPlayerJoined(string playerId)
        {
            
        }

        private void OnPlayerLeaving(string playerId)
        {
            
        }

        private void OnPlayerPropertyChanged()
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
            
            PlayerPropertiesChanged?.Invoke(playersInfoList);
        }

        public void ClearSession()
        {
            Session.PlayerJoined -= OnPlayerJoined;
            Session.PlayerLeaving -= OnPlayerLeaving;
            Session.PlayerPropertiesChanged -= OnPlayerPropertyChanged;
            
            _isInitialized = false;
            Session = null;
        }

        private void OnDestroy()
        {
            if (Session == null)
                return;
            
            Session.PlayerJoined -= OnPlayerJoined;
            Session.PlayerLeaving -= OnPlayerLeaving;
            Session.PlayerPropertiesChanged -= OnPlayerPropertyChanged;
        }
    }
}