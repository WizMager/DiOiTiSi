using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace Authoring
{
    public class DropItemIconAuthoring : MonoBehaviour
    {
        public Material[] dropItemMaterials;
        
        private class DropItemIconAuthoringBaker : Baker<DropItemIconAuthoring>
        {
            public override void Bake(DropItemIconAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var mesh = GetComponent<MeshFilter>().sharedMesh;

                DependsOn(mesh);
                
                AddSharedComponent(entity, new RenderMeshArray(authoring.dropItemMaterials, new []{mesh}));
                AddComponent(entity, MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));
            }
        }
    }
}