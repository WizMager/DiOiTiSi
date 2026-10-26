using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Systems
{
    [UpdateInGroup(typeof(GameplaySimulationSystemGroup))]
    public partial struct SpawnCooldownCounterSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (enemySpawnCooldown, canSpawnEnabled) in SystemAPI.Query<RefRW<EnemySpawnCooldownComponent>, EnabledRefRW<CanSpawnTag>>().WithDisabled<CanSpawnTag>())
            {
                if (enemySpawnCooldown.ValueRO.CurrentTime > 0)
                {
                    enemySpawnCooldown.ValueRW.CurrentTime = math.max(0, enemySpawnCooldown.ValueRO.CurrentTime - SystemAPI.Time.DeltaTime);
                    
                    if (enemySpawnCooldown.ValueRO.CurrentTime <= 0)
                    {
                        canSpawnEnabled.ValueRW = true;
                    }
                }
                else
                {
                    canSpawnEnabled.ValueRW = true;
                }
            }
        }
    }
}