using Components;
using Enums;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class DropItemAuthoring : MonoBehaviour
    {
        public GameObject iconObject;
        
        private class DropItemAuthoringBaker : Baker<DropItemAuthoring>
        {
            public override void Bake(DropItemAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new DropItemComponent
                {
                    ItemType = EDropItem.None,
                    Icon = GetEntity(authoring.iconObject, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}