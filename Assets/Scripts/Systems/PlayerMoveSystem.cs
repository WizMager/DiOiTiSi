using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace Systems
{
    public partial struct PlayerMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (input, physicsVelocity, moveSpeed) 
                     in SystemAPI.Query<RefRO<InputComponent>, RefRW<PhysicsVelocity>, RefRO<MoveSpeedComponent>>().WithAll<PlayerTag>())
            {
                var direction = new float3(input.ValueRO.MoveDirection.x, 0, input.ValueRO.MoveDirection.y);

                if (math.lengthsq(direction) > 1f)
                    direction = math.normalize(direction);

                physicsVelocity.ValueRW.Linear = new float3(direction.x * moveSpeed.ValueRO.Value,
                    physicsVelocity.ValueRO.Linear.y, direction.z * moveSpeed.ValueRO.Value);
            }
        }
    }
}