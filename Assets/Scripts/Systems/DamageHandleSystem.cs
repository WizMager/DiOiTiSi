using Components;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    [UpdateInGroup(typeof(GameplaySimulationSystemGroup))]
    public partial struct DamageHandleSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GainExperienceBuffer>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var experienceDropLookup = SystemAPI.GetComponentLookup<ExperienceDropComponent>(true);
            var playerExperienceSingleton = SystemAPI.GetSingletonBuffer<GainExperienceBuffer>();
            
            foreach (var (damageBuffer, health, entity) 
                     in SystemAPI.Query<DynamicBuffer<DamageBuffer>, RefRW<HealthComponent>>().WithEntityAccess())
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

                if (experienceDropLookup.HasComponent(entity)){
                    playerExperienceSingleton.Add(new GainExperienceBuffer
                    {
                        Value = experienceDropLookup[entity].Value
                    });
                }
                
                ecb.DestroyEntity(entity);
            }
        }
    }
}