using Components;
using Enums;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

namespace Systems
{
    [UpdateInGroup(typeof(GameplaySimulationSystemGroup))]
    [UpdateAfter(typeof(DamageHandleSystem))]
    public partial struct SpawnDropItemSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<DropItemMaterialsIdBuffer>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var dropItemMaterialIdBuffer = SystemAPI.GetSingletonBuffer<DropItemMaterialsIdBuffer>();
            
            foreach (var (spawnPositions, dropItemPrefab, dropItemData, randomComponent) 
                     in SystemAPI.Query<DynamicBuffer<SpawnDropItemPositionsBuffer>, RefRO<DropItemPrefabComponent>, DynamicBuffer<DropItemsDataBuffer>, RefRW<RandomComponent>>())
            {
                var sumWeight = 0f;
                foreach (var item in dropItemData)
                {
                    sumWeight += item.Weight;
                }

                var positions = spawnPositions.ToNativeArray(Allocator.Temp);
                spawnPositions.Clear();
                
                foreach (var position in positions)
                {
                    var random = randomComponent.ValueRO.Value;
                    var pickWeight = random.NextFloat() * sumWeight;
                    randomComponent.ValueRW.Value = random;
                    var tempWeight = 0f;
                    var choseIndex = 0;

                    for (var i = 0; i < dropItemData.Length; i++)
                    {
                        tempWeight += dropItemData[i].Weight;

                        if (pickWeight > tempWeight) 
                            continue;
                    
                        choseIndex = i;
                        break;
                    }

                    var type = dropItemData[choseIndex].Type;
                    var dropItem = state.EntityManager.Instantiate(dropItemPrefab.ValueRO.Value);
                    SystemAPI.SetComponent(dropItem, LocalTransform.FromPosition(position.Value));
                    var dropItemComponent = SystemAPI.GetComponentRW<DropItemComponent>(dropItem);
                    dropItemComponent.ValueRW.ItemType = type;
                    var icon = dropItemComponent.ValueRO.Icon;
                    var materialMeshInfo = SystemAPI.GetComponent<MaterialMeshInfo>(icon);
                    var indexMat = (int)type - 1;
                    materialMeshInfo.MaterialID = dropItemMaterialIdBuffer[indexMat].Value;
            
                    SystemAPI.SetComponent(icon, materialMeshInfo);
                }

                positions.Dispose();
            }
        }
    }
}