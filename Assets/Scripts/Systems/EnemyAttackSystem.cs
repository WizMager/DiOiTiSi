using Components;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    [UpdateBefore(typeof(DamageHandleSystem))]
    [UpdateAfter(typeof(CooldownCounterSystem))]
    [UpdateAfter(typeof(EnemyMoveSystem))]
    public partial struct EnemyAttackSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (attackTarget, canAttack, damage, cooldown) 
                     in SystemAPI.Query<RefRO<AttackTargetComponent>, EnabledRefRW<CanAttackTag>, RefRO<DamageComponent>, RefRW<AttackCooldownComponent>>().WithAll<EnemyTag>())
            {
                cooldown.ValueRW.CurrentTime = cooldown.ValueRO.Cooldown;
                canAttack.ValueRW = false;

                var damageBuffer = SystemAPI.GetBuffer<DamageBuffer>(attackTarget.ValueRO.Value);
                damageBuffer.Add(new DamageBuffer
                {
                    Value = damage.ValueRO.Value
                });
            }
        }
    }
}