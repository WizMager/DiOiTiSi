using Components;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    public partial struct SpawnerSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (cooldown, cansSpawn, enemiesPrefabs) 
                     in SystemAPI.Query<RefRW<EnemySpawnCooldownComponent>, RefRW<CanSpawnTag>, DynamicBuffer<EnemyPrefabBuffer>>()
                         .WithAll<SpawnerTag>())
            {
                
            }
        }
    }
}