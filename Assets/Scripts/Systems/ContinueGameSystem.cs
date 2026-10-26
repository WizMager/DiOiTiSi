using Components;
using Unity.Entities;
using Unity.Physics.Systems;

namespace Systems
{
    public partial struct ContinueGameSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (continueEnabled, isPauseEnabled, levelUp) in SystemAPI.Query<EnabledRefRW<ContinueGameTag>, EnabledRefRW<IsPauseTag>, RefRW<LevelUpComponent>>())
            {
                continueEnabled.ValueRW = false;
                isPauseEnabled.ValueRW = false;
                levelUp.ValueRW.Value--;
                
                state.World.GetExistingSystemManaged<GameplaySimulationSystemGroup>().Enabled = true;
                state.World.GetExistingSystemManaged<PhysicsSystemGroup>().Enabled = true;
                state.World.GetExistingSystemManaged<FixedStepSimulationSystemGroup>().Enabled = true;
            }
        }
    }
}