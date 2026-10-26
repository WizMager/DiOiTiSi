using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [UpdateInGroup(typeof(GameplaySimulationSystemGroup))]
    public partial struct SpawnerSpawnSystem : ISystem
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
            
            foreach (var (cooldown, canSpawn, enemiesPrefabs, randomComponent, localTransform) 
                     in SystemAPI.Query<RefRW<EnemySpawnCooldownComponent>, EnabledRefRW<CanSpawnTag>, DynamicBuffer<EnemyPrefabBuffer>, RefRW<RandomComponent>, RefRO<LocalTransform>>()
                         .WithAll<SpawnerTag>())
            {
                cooldown.ValueRW.CurrentTime = cooldown.ValueRO.Cooldown;
                canSpawn.ValueRW = false;
                
                var random = randomComponent.ValueRW.Value;
                
                var tempWeight = 0f;
                foreach (var enemyPrefab in enemiesPrefabs)
                {
                    tempWeight += enemyPrefab.Weight;
                }
                
                var pickWeight = random.NextFloat() * tempWeight;
                randomComponent.ValueRW.Value = random;
                tempWeight = 0;
                var choseIndex = 0;

                for (var i = 0; i < enemiesPrefabs.Length; i++)
                {
                    tempWeight += enemiesPrefabs[i].Weight;

                    if (pickWeight > tempWeight) 
                        continue;
                    
                    choseIndex = i;
                    break;
                }
                
                var enemyEntity = ecb.Instantiate(enemiesPrefabs[choseIndex].Prefab);

                ecb.SetComponent(enemyEntity, new LocalTransform
                {
                    Position = localTransform.ValueRO.Position,
                    Rotation = quaternion.identity,
                    Scale = 1
                });
            }
        }
    }
}