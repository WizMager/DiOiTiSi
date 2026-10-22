using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public partial struct EnemyGlobalSpawnSystem : ISystem
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
            
            foreach (var (cooldown, enemyPrefabs, randomComponent, spawnDistance, canSpawn) 
                     in SystemAPI.Query<RefRW<EnemySpawnCooldownComponent>, DynamicBuffer<EnemyPrefabBuffer>, RefRW<RandomComponent>, RefRO<EnemySpawnDistanceComponent>, EnabledRefRW<CanSpawnTag>>()
                         .WithAny<SpawnerOfSpawnerTag, SingleEnemySpawnerTag>())
            {
                cooldown.ValueRW.CurrentTime = cooldown.ValueRO.Cooldown;
                canSpawn.ValueRW = false;

                var minX = float.MaxValue;
                var maxX = float.MinValue;
                var minZ = float.MaxValue;
                var maxZ = float.MinValue;

                foreach (var localTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerTag>())
                {
                    var playerPosition = localTransform.ValueRO.Position;
                    minX = math.min(minX, playerPosition.x);
                    maxX = math.max(maxX, playerPosition.x);
                    minZ = math.min(minZ, playerPosition.z);
                    maxZ = math.max(maxZ, playerPosition.z);
                }

                var distanceX = spawnDistance.ValueRO.LeftRightDistance;
                var distanceZ = spawnDistance.ValueRO.TopBottomDistance;

                minX -= distanceX;
                maxX += distanceX;
                minZ -= distanceZ;
                maxZ += distanceZ;

                var width = maxX - minX;
                var height = maxZ - minZ;
                var perimeter = 2f * (width + height);
                var random = randomComponent.ValueRW.Value;
                var randomDotInOnPerimeter = random.NextFloat() * perimeter;
                

                float3 position;
                if (randomDotInOnPerimeter < width)
                {
                    position = new float3(minX + randomDotInOnPerimeter, 0, minZ);
                }
                else if ((randomDotInOnPerimeter -= width) < height)
                {
                    position = new float3(maxX, 0, minZ + randomDotInOnPerimeter);
                }
                else if ((randomDotInOnPerimeter -= height) < width)
                {
                    position = new float3(maxX - randomDotInOnPerimeter, 0, maxZ);
                }
                else
                {
                    randomDotInOnPerimeter -= width;
                    position = new float3(minX, 0, maxZ - randomDotInOnPerimeter);
                }

                var tempWeight = 0f;
                foreach (var enemyPrefab in enemyPrefabs)
                {
                    tempWeight += enemyPrefab.Weight;
                }
                
                var pickWeight = random.NextFloat() * tempWeight;
                randomComponent.ValueRW.Value = random;
                tempWeight = 0;
                var choseIndex = 0;

                for (var i = 0; i < enemyPrefabs.Length; i++)
                {
                    tempWeight += enemyPrefabs[i].Weight;

                    if (pickWeight > tempWeight) 
                        continue;
                    
                    choseIndex = i;
                    break;
                }
                
                var enemyEntity = ecb.Instantiate(enemyPrefabs[choseIndex].Prefab);

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