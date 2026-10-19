using Components;
using Data;
using Enums;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Authoring
{
    public class EnemySpawnAuthoring : MonoBehaviour
    {
        public EnemySpawnData enemySpawnData;
        
        private class EnemyPrefabAuthoringBaker : Baker<EnemySpawnAuthoring>
        {
            public override void Bake(EnemySpawnAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new EnemyPrefabComponent
                {
                    Value = GetEntity(authoring.enemySpawnData.GetEnemy(EEnemyType.Melee), TransformUsageFlags.Dynamic)
                });
                
                AddComponent(entity, new EnemySpawnCooldownComponent
                {
                    Cooldown = authoring.enemySpawnData.SingleEnemySpawnCooldown,
                    CurrentTime = 0
                });
                AddComponent(entity, new RandomComponent
                {
                    Value = Random.CreateFromIndex((uint)authoring.GetHashCode()),
                });
            }
        }
    }
}