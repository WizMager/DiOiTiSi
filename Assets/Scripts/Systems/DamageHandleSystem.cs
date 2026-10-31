using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    [UpdateInGroup(typeof(GameplaySimulationSystemGroup))]
    public partial struct DamageHandleSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<RandomizerTag>();
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GainExperienceBuffer>();
            state.RequireForUpdate<SpawnDropItemPositionsBuffer>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var playerExperienceSingleton = SystemAPI.GetSingletonBuffer<GainExperienceBuffer>();
            var spawnDropItemPositionBufferSingleton = SystemAPI.GetSingletonBuffer<SpawnDropItemPositionsBuffer>();
            var randomizerEntity = SystemAPI.GetSingletonEntity<RandomizerTag>();
            var randomizer = SystemAPI.GetComponentRW<RandomComponent>(randomizerEntity);
            
            foreach (var (damageBuffer, health, transform,  entity) 
                     in SystemAPI.Query<DynamicBuffer<DamageBuffer>, RefRW<HealthComponent>, RefRO<LocalToWorld>>().WithEntityAccess())
            {
                if (damageBuffer.IsEmpty)
                    continue;

                var damage = 0f;
                
                foreach (var damageValue in damageBuffer)
                {
                    damage += damageValue.Value;
                }
                
                damageBuffer.Clear();

                health.ValueRW.Value -= damage;

                if (health.ValueRO.Value > 0)
                    continue;

                
                if (SystemAPI.HasComponent<ExperienceDropComponent>(entity)){
                    playerExperienceSingleton.Add(new GainExperienceBuffer
                    {
                        Value = SystemAPI.GetComponentRO<ExperienceDropComponent>(entity).ValueRO.Value
                    });
                    
                }

                if (SystemAPI.HasComponent<DropItemDropChanceComponent>(entity))
                {
                    var random = randomizer.ValueRO.Value;
                    var chance = SystemAPI.GetComponentRO<DropItemDropChanceComponent>(entity).ValueRO.Value;
                    var dropChance = random.NextFloat(0, 1f);
                    randomizer.ValueRW.Value = random;

                    if (dropChance <= chance)
                    {
                        spawnDropItemPositionBufferSingleton.Add(new SpawnDropItemPositionsBuffer
                        {
                            Value = transform.ValueRO.Position
                        });
                    }
                }
                
                ecb.DestroyEntity(entity);
            }
        }
    }
}