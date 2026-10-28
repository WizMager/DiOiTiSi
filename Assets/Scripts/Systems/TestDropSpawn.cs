using Components;
using Enums;
using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;

namespace Systems
{
    [UpdateAfter(typeof(InitializeDropItemMaterialsSystem))]
    public partial struct TestDropSpawn : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<DropItemPrefabComponent>();
            state.RequireForUpdate<DropItemMaterialsIdBuffer>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var dropItemPrefab = SystemAPI.GetSingleton<DropItemPrefabComponent>();
            var dropItemEntity = state.EntityManager.Instantiate(dropItemPrefab.Value);
            
            var dropItemComponent = SystemAPI.GetComponentRW<DropItemComponent>(dropItemEntity);
            dropItemComponent.ValueRW.ItemType = EDropItem.MoveSpeed;

            var icon = dropItemComponent.ValueRO.Icon;
            var index = (int)dropItemComponent.ValueRO.ItemType - 1;

            var materialMeshInfo = SystemAPI.GetComponent<MaterialMeshInfo>(icon);
            var dropItemMaterialIdBuffer = SystemAPI.GetSingletonBuffer<DropItemMaterialsIdBuffer>();
            materialMeshInfo.MaterialID = dropItemMaterialIdBuffer[index].Value;
            
            SystemAPI.SetComponent(icon, materialMeshInfo);

            state.Enabled = false;
        }
    }
}