using Components;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    [UpdateInGroup(typeof(GameplaySimulationSystemGroup))]
    [UpdateAfter(typeof(DamageHandleSystem))]
    public partial struct ExperienceGainSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var levelUpComponent = SystemAPI.GetSingletonRW<LevelUpComponent>();
            
            foreach (var (expBuffer, expComponent) 
                     in SystemAPI.Query<DynamicBuffer<GainExperienceBuffer>, RefRW<ExperienceComponent>>())
            {
                if (expBuffer.Length <= 0)
                    return;

                var exp = 0f;
                foreach (var gainExperienceBuffer in expBuffer)
                {
                    exp += gainExperienceBuffer.Value;
                }

                if (expComponent.ValueRO.Current + exp >= expComponent.ValueRO.LevelUp)
                {
                    var needExp = expComponent.ValueRO.LevelUp - expComponent.ValueRO.Current;
                    exp -= needExp;
                    expComponent.ValueRW.Current = exp;
                    expComponent.ValueRW.LevelUp *= 2;
                    levelUpComponent.ValueRW.Value++;
                }
                else
                {
                    expComponent.ValueRW.Current += exp;
                }
                
                expBuffer.Clear();
            }
        }
    }
}