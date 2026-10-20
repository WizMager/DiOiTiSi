using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Systems
{
    public partial struct CooldownCounterSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (attackCooldown, canAttack) 
                     in SystemAPI.Query<RefRW<AttackCooldownComponent>, EnabledRefRW<CanAttackTag>>().WithDisabled<CanAttackTag>())
            {
                if (attackCooldown.ValueRO.CurrentTime > 0)
                {
                    attackCooldown.ValueRW.CurrentTime = math.max(0, attackCooldown.ValueRO.CurrentTime - SystemAPI.Time.DeltaTime);
                    
                    if (attackCooldown.ValueRO.CurrentTime <= 0)
                    {
                        canAttack.ValueRW = true;
                    }
                }
                else
                {
                    canAttack.ValueRW = true;
                }
            }
        }
    }
}