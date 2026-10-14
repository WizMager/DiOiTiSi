using Components;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct CameraFollowSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CameraComponent>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var camera = SystemAPI.GetSingleton<CameraComponent>().Value.Value;

            foreach (var localToWorld in SystemAPI.Query<RefRO<LocalToWorld>>().WithAll<PlayerTag>())
            {
                var playerPosition = localToWorld.ValueRO.Position;
                var cameraPosition = camera.transform.position;
                camera.transform.position = new Vector3(playerPosition.x, cameraPosition.y, playerPosition.z);
            }
        }
    }
}
