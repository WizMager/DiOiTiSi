using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class DropItemIconMaterialsAuthoring : MonoBehaviour
    {
        public Material[] dropItemMaterials;
        
        private class DropItemIconMaterialsAuthoringBaker : Baker<DropItemIconMaterialsAuthoring>
        {
            public override void Bake(DropItemIconMaterialsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                var materialBuffer = AddBuffer<DropItemMaterialsBuffer>(entity);
                AddBuffer<DropItemMaterialsIdBuffer>(entity);
                
                foreach (var material in authoring.dropItemMaterials)
                {
                    if (material == null)
                        continue;

                    DependsOn(material);
                    materialBuffer.Add(new DropItemMaterialsBuffer
                    {
                        Value = material
                    });
                }
            }
        }
    }
}