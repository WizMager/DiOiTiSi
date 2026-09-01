using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace Gameplay.Utils
{
    public class CameraHelper : MonoBehaviour
    {
        public static CameraHelper Instance { get; private set; }
        
        [field:SerializeField] public Camera PlayerCamera { get; private set; }

        private World _clientWorld;
        private EntityQuery _playerQuery;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            if (PlayerCamera == null)
                PlayerCamera = GetComponent<Camera>();
        }

        private void Start()
        {
            _clientWorld = ClientServerBootstrap.ClientWorld;
            _playerQuery = _clientWorld.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<GhostOwnerIsLocal>(),
                ComponentType.ReadOnly<LocalTransform>());
        }

        private void LateUpdate()
        {
            if (_playerQuery.IsEmpty)
                return;

            var playerPosition = _playerQuery.GetSingleton<LocalTransform>().Position;
            transform.position = new Vector3(playerPosition.x, 40f, playerPosition.z);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}