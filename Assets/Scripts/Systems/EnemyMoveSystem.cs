using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Systems
{
    public partial struct EnemyMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (attackDistance, transform, physicsVelocity, moveSpeed, attackTarget, attackTargetEnabled) 
                     in SystemAPI.Query<RefRO<AttackDistanceComponent>, RefRO<LocalTransform>, RefRW<PhysicsVelocity>, RefRO<MoveSpeedComponent>, RefRW<AttackTargetComponent>, EnabledRefRW<AttackTargetComponent>>()
                         .WithNone<PlayerTag>()
                         .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
            {
                var nearestPlayerPosition =  float3.zero;
                var distanceSqr = float.MaxValue;
                var playerEntity = Entity.Null;
                foreach (var (localTransform, entity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerTag>().WithEntityAccess())
                {
                    var distance = math.distancesq(localTransform.ValueRO.Position, transform.ValueRO.Position);
                    if (distance >= distanceSqr)
                        continue;
                    
                    distanceSqr = distance;
                    nearestPlayerPosition = localTransform.ValueRO.Position;
                    playerEntity = entity;
                }

                if (attackTarget.ValueRO.Value != playerEntity)
                {
                    attackTarget.ValueRW.Value = playerEntity;
                }
                
                if (distanceSqr <= attackDistance.ValueRO.Value * attackDistance.ValueRO.Value)
                {
                    physicsVelocity.ValueRW.Linear = float3.zero;

                    attackTargetEnabled.ValueRW = true;
                    
                    
                }
                else
                {
                    attackTargetEnabled.ValueRW = false;
                    
                    var moveDirection = nearestPlayerPosition - transform.ValueRO.Position;
                    moveDirection.y = 0;
                    moveDirection = math.normalize(moveDirection);
                    physicsVelocity.ValueRW.Linear = new float3(moveDirection.x * moveSpeed.ValueRO.Value, 0, moveDirection.z * moveSpeed.ValueRO.Value);
                }
            }
        }
    }
}