using Components;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Authoring
{
    public class EnemyPrefabAuthoring : MonoBehaviour
    {
        public GameObject enemyPrefab;
        
        private class EnemyPrefabAuthoringBaker : Baker<EnemyPrefabAuthoring>
        {
            public override void Bake(EnemyPrefabAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new EnemyPrefabComponent
                {
                    Value = GetEntity(authoring.enemyPrefab, TransformUsageFlags.Dynamic)
                });
                
                AddComponent<EnemySpawnCooldownComponent>(entity);
                AddComponent(entity, new RandomComponent
                {
                    Value = Random.CreateFromIndex((uint)authoring.GetHashCode()),
                });
            }
        }
    }
}