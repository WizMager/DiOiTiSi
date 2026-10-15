using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class ProjectilePrefabAuthoring : MonoBehaviour
    {
        public GameObject projectilePrefab;
        
        private class ProjectilePrefabAuthoringBaker : Baker<ProjectilePrefabAuthoring>
        {
            public override void Bake(ProjectilePrefabAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new ProjectilePrefabComponent
                {
                    Value = GetEntity(authoring.projectilePrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}