using Components;
using Data;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class DropItemPrefabAuthoring : MonoBehaviour
    {
        public GameObject dropItemPrefab;
        public DropItemsData dropItemsData;
        
        private class DropItemAuthoringBaker : Baker<DropItemPrefabAuthoring>
        {
            public override void Bake(DropItemPrefabAuthoring prefabAuthoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent<RandomComponent>(entity);
                SetComponentEnabled<RandomComponent>(entity, false);
                AddComponent(entity, new DropItemPrefabComponent
                {
                    Value = GetEntity(prefabAuthoring.dropItemPrefab, TransformUsageFlags.Dynamic)
                });
                var dropItemsDataBuffer = AddBuffer<DropItemsDataBuffer>(entity);

                foreach (var dropItemDo in prefabAuthoring.dropItemsData.DropItems)
                {
                    dropItemsDataBuffer.Add(new DropItemsDataBuffer
                    {
                        Type = dropItemDo.type,
                        Weight = dropItemDo.weight
                    });
                }
            }
        }
    }
}