using Components;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    [UpdateInGroup(typeof(GameplaySimulationSystemGroup))]
    [UpdateBefore(typeof(DamageHandleSystem))]
    [UpdateAfter(typeof(CooldownCounterSystem))]
    [UpdateAfter(typeof(EnemyMoveSystem))]
    public partial struct EnemyAttackSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (attackTarget, canAttack, damage, cooldown) 
                     in SystemAPI.Query<RefRO<AttackTargetComponent>, EnabledRefRW<CanAttackTag>, RefRO<DamageComponent>, RefRW<AttackCooldownComponent>>().WithAll<EnemyTag>())
            {
                var target = attackTarget.ValueRO.Value;

                if (target == Entity.Null || !SystemAPI.Exists(target))
                    continue;
                
                cooldown.ValueRW.CurrentTime = cooldown.ValueRO.Cooldown;
                canAttack.ValueRW = false;

                var damageBuffer = SystemAPI.GetBuffer<DamageBuffer>(target);
                damageBuffer.Add(new DamageBuffer
                {
                    Value = damage.ValueRO.Value
                });
            }
        }
    }
}