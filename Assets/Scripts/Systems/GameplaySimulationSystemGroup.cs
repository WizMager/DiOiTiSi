using Unity.Entities;

namespace Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(InputSystem))]
    public partial class GameplaySimulationSystemGroup : ComponentSystemGroup {}
}