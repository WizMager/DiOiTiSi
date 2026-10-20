using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Systems
{
    [UpdateAfter(typeof(InputSystem))]
    [UpdateAfter(typeof(CooldownCounterSystem))]
    public partial struct ShootSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<ProjectilePrefabComponent>();
            state.RequireForUpdate<InputComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) 
        {
            var projectilePrefab = SystemAPI.GetSingleton<ProjectilePrefabComponent>();
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            
            foreach (var (shootCooldown, shootPoint, input, canAttack) 
                     in SystemAPI.Query<RefRW<AttackCooldownComponent>, RefRO<ShootPointComponent>, RefRW<InputComponent>, EnabledRefRW<CanAttackTag>>().WithAll<PlayerTag>())
            {
                if (!input.ValueRO.Attack)
                {
                    continue;
                }
                
                input.ValueRW.Attack = false;
                canAttack.ValueRW = false;
                
                var shootTransform = SystemAPI.GetComponent<LocalToWorld>(shootPoint.ValueRO.Value);
                shootCooldown.ValueRW.CurrentTime = shootCooldown.ValueRO.Cooldown;
                
                var projectile = ecb.Instantiate(projectilePrefab.Value);
                var localTransform = new LocalTransform
                {
                    Position = shootTransform.Position,
                    Scale = 1,
                    Rotation = shootTransform.Rotation

                };
                    
                ecb.SetComponent(projectile, localTransform);
                ecb.SetComponent(projectile, new PhysicsVelocity
                {
                    Linear = math.forward(shootTransform.Rotation) * 10f
                });
                ecb.AddComponent<ProjectileTag>(projectile);
                ecb.AddComponent(projectile, new DamageComponent
                {
                    Value = 25
                });
            }
        }
    }
}