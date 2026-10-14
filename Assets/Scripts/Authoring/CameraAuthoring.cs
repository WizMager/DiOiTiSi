using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class CameraAuthoring : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        
        private void Start()
        {
            if (_camera == null)
                _camera = Camera.main;
            
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var entity = entityManager.CreateEntity();
            entityManager.AddComponentData(entity, new CameraComponent
            {
                Value = _camera
            });
            entityManager.SetName(entity, "Camera");
        }
    }
}