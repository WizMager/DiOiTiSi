using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class DropItemPrefabAuthoring : MonoBehaviour
    {
        public GameObject dropItemPrefab;
        
        private class DropItemAuthoringBaker : Baker<DropItemPrefabAuthoring>
        {
            public override void Bake(DropItemPrefabAuthoring prefabAuthoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new DropItemPrefabComponent
                {
                    Value = GetEntity(prefabAuthoring.dropItemPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}