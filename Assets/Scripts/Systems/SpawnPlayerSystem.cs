using Components;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    public partial struct SpawnPlayerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<PlayerPrefabComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var playerPrefab = SystemAPI.GetSingleton<PlayerPrefabComponent>();

            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            ecb.Instantiate(playerPrefab.Value);

            state.Enabled = false;
        }
    }
}