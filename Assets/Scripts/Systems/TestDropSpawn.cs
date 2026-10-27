using Components;
using Enums;
using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;

namespace Systems
{
    public partial struct TestDropSpawn : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<DropItemPrefabComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {

            var dropItemPrefab = SystemAPI.GetSingleton<DropItemPrefabComponent>();

            var dropItemEntity = state.EntityManager.Instantiate(dropItemPrefab.Value);

            var dropItemComponent = SystemAPI.GetComponentRW<DropItemComponent>(dropItemEntity);
            dropItemComponent.ValueRW.ItemType = EDropItem.MoveSpeed;
            var index = (int)dropItemComponent.ValueRO.ItemType - 1;
            SystemAPI.SetComponent(dropItemComponent.ValueRO.Icon, MaterialMeshInfo.FromRenderMeshArrayIndices(index, 0));

            state.Enabled = false;
        }
    }
}