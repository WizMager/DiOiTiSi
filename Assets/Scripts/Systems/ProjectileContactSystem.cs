using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Systems
{
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(PhysicsSimulationGroup))]
    public partial struct ProjectileContactSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<SimulationSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new ProjectileTriggerJob
            {
                ProjectileLookup = SystemAPI.GetComponentLookup<ProjectileTag>(true),
                EnemyLookup = SystemAPI.GetComponentLookup<EnemyTag>(true),
                DamageLookup = SystemAPI.GetComponentLookup<DamageComponent>(true),
                Ecb = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>()
                    .CreateCommandBuffer(state.WorldUnmanaged)
                    .AsParallelWriter()
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }
        
        public struct ProjectileTriggerJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentLookup<ProjectileTag> ProjectileLookup;
            [ReadOnly] public ComponentLookup<EnemyTag> EnemyLookup;
            [ReadOnly] public ComponentLookup<DamageComponent> DamageLookup;
            
            public EntityCommandBuffer.ParallelWriter Ecb;
            
            public void Execute(TriggerEvent triggerEvent)
            {
                var a = triggerEvent.EntityA;
                var b = triggerEvent.EntityB;

                var projectile = ProjectileLookup.HasComponent(a) ? a : b;
                var other = projectile == a ? b : a;

                if (!ProjectileLookup.HasComponent(projectile))
                    return;
                
                Ecb.DestroyEntity(projectile.Index, projectile);
                
                if (!EnemyLookup.HasComponent(other))
                    return;
                
                Ecb.AppendToBuffer(other.Index, other, new DamageBuffer
                {
                    Value = DamageLookup[projectile].Value
                });
            }
        }
    }
}