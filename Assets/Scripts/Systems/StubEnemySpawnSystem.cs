using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public partial struct StubEnemySpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            
            foreach (var (cooldown, enemyPrefab, randomComponent) 
                     in SystemAPI.Query<RefRW<EnemySpawnCooldownComponent>, RefRO<EnemyPrefabComponent>, RefRW<RandomComponent>>())
            {
                if (cooldown.ValueRO.Value > 0)
                {
                    cooldown.ValueRW.Value -= SystemAPI.Time.DeltaTime;
                }
                else
                {
                    cooldown.ValueRW.Value = 5;

                    var enemyEntity = ecb.Instantiate(enemyPrefab.ValueRO.Value);
                    var random = randomComponent.ValueRW.Value;
                    var dir = random.NextFloat2Direction();
                    var r = 5 * math.sqrt(random.NextFloat());
                    randomComponent.ValueRW.Value = random;
                    var position = new float3(dir.x * r, 0f, dir.y * r);
                    
                    ecb.SetComponent(enemyEntity, new LocalTransform
                    {
                        Position = position,
                        Rotation = quaternion.identity,
                        Scale = 1
                    });
                }
            }
        }
    }
}