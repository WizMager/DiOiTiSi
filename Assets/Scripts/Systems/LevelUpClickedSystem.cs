using Components;
using Unity.Entities;
using Unity.Physics.Systems;

namespace Systems
{
    [UpdateInGroup(typeof(GameplaySimulationSystemGroup))]
    public partial struct LevelUpClickedSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<LevelUpComponent>();
            state.RequireForUpdate<IsPauseTag>();
            state.RequireForUpdate<InputComponent>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var inputComponent = SystemAPI.GetSingletonRW<InputComponent>();
            
            if (!inputComponent.ValueRO.LevelUpMenu)
                return;

            inputComponent.ValueRW.LevelUpMenu = false;

            state.World.GetExistingSystemManaged<GameplaySimulationSystemGroup>().Enabled = false;
            state.World.GetExistingSystemManaged<PhysicsSystemGroup>().Enabled = false;
            state.World.GetExistingSystemManaged<FixedStepSimulationSystemGroup>().Enabled = false;
            
            var gameplayEntity = SystemAPI.GetSingletonEntity<LevelUpComponent>();
            SystemAPI.SetComponentEnabled<IsPauseTag>(gameplayEntity, true);
        }
    }
}