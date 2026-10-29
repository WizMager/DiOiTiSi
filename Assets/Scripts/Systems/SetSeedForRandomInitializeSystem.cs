using Components;
using Unity.Entities;
using Unity.Mathematics;

namespace Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SetSeedForRandomInitializeSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (randomComponent, enabledRandomComponent, entity) 
                     in SystemAPI.Query<RefRW<RandomComponent>, EnabledRefRW<RandomComponent>>().WithDisabled<RandomComponent>().WithEntityAccess())
            {
                randomComponent.ValueRW.Value =
                    Random.CreateFromIndex((uint)(entity.Index + System.Environment.TickCount));
                
                enabledRandomComponent.ValueRW = true;
            }
        }
    }
}