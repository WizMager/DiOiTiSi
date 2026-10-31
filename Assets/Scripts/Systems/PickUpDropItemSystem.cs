using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

namespace Systems
{
    public partial struct PickUpDropItemSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationSingleton>();
            state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new DropItemTriggerJob
            {
                PlayerLookup = SystemAPI.GetComponentLookup<PlayerTag>(),
                DropItemComponentLookup = SystemAPI.GetComponentLookup<DropItemComponent>(),
                Ecb = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>()
                    .CreateCommandBuffer(state.WorldUnmanaged)
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }
        
        private struct DropItemTriggerJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentLookup<PlayerTag> PlayerLookup;
            [ReadOnly] public ComponentLookup<DropItemComponent> DropItemComponentLookup;

            public EntityCommandBuffer Ecb;
            
            public void Execute(TriggerEvent triggerEvent)
            {
                var a = triggerEvent.EntityA;
                var b = triggerEvent.EntityB;

                var playerEntity = PlayerLookup.HasComponent(a) ? a : b;
                var dropItemEntity = DropItemComponentLookup.HasComponent(a) ? a : b;

                if (!PlayerLookup.HasComponent(playerEntity))
                    return;
                if (!DropItemComponentLookup.HasComponent(dropItemEntity))
                    return;
                
                Ecb.AppendToBuffer(playerEntity, new PickedUpDropItemComponent
                    {
                        ItemType = DropItemComponentLookup[dropItemEntity].ItemType
                    }
                );
                
                Ecb.DestroyEntity(dropItemEntity);
            }
        }
    }
}