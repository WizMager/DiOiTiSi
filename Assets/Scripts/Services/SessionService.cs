using JetBrains.Annotations;
using Unity.Services.Multiplayer;
using UnityEngine;

namespace Services
{
    public class SessionService : MonoBehaviour
    {
        private bool _isInitialized;
        
        public static SessionService Instance { get; private set; }

        [CanBeNull] public ISession HostSession { get; private set; }
        
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
            if (_isInitialized)
                return;
            
            _isInitialized = true;
            HostSession = session;
        }

        public void ClearSession()
        {
            _isInitialized = false;
            HostSession = null;
        }
    }
}