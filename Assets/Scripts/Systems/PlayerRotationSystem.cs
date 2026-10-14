using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(PhysicsSystemGroup))]
    public partial struct PlayerRotationSystem : ISystem
    {
        private static readonly CollisionFilter CollisionGround = new()
        {
            BelongsTo = CollisionFilter.Default.BelongsTo,
            CollidesWith = 1u << 0,
            GroupIndex = 0
        };

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;

            foreach (var (input, transform, physicsVelocity) in SystemAPI
                         .Query<RefRO<InputComponent>, RefRW<LocalTransform>, RefRW<PhysicsVelocity>>()
                         .WithAll<PlayerTag>())
            {
                var raycast = new RaycastInput
                {
                    Filter = CollisionGround,
                    Start = input.ValueRO.AimRayStart,
                    End = input.ValueRO.AimRayEnd
                };

                if (!collisionWorld.CastRay(raycast, out var hit))
                    continue;

                var lookDirection = hit.Position - transform.ValueRO.Position;
                lookDirection.y = 0f;

                if (math.lengthsq(lookDirection) < 0.0001f)
                    continue;

                transform.ValueRW.Rotation = quaternion.LookRotationSafe(lookDirection, math.up());
                physicsVelocity.ValueRW.Angular = float3.zero;
            }
        }
    }
}
