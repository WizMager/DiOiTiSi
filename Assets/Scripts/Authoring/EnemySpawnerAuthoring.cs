using System.Collections.Generic;
using Components;
using Data.DataObject;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Authoring
{
    public class EnemySpawnerAuthoring : MonoBehaviour
    {
        public List<EnemyPrefabDo> enemies;
        public float spawnCooldown = 7;
        public float health = 100;
        public float experienceDrop = 50;
        
        private class EnemySpawnerAuthoringBaker : Baker<EnemySpawnerAuthoring>
        {
            public override void Bake(EnemySpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new EnemySpawnCooldownComponent
                {
                    Cooldown = authoring.spawnCooldown,
                    CurrentTime = authoring.spawnCooldown
                });
                AddComponent(entity, new HealthComponent
                {
                    Value = authoring.health
                });
                AddComponent<RandomComponent>(entity);
                SetComponentEnabled<RandomComponent>(entity, false);
                AddComponent<CanSpawnTag>(entity);
                SetComponentEnabled<CanSpawnTag>(entity, false);
                AddComponent<SpawnerTag>(entity);
                AddComponent(entity, new ExperienceDropComponent
                {
                    Value = authoring.experienceDrop
                });
                AddBuffer<DamageBuffer>(entity);

                var enemiesBuffer = AddBuffer<EnemyPrefabBuffer>(entity);
                foreach (var enemyPrefabDo in authoring.enemies)
                {
                    if (!enemyPrefabDo.prefab)
                        continue;
                    
                    enemiesBuffer.Add(new EnemyPrefabBuffer
                    {
                        Weight = enemyPrefabDo.weight,
                        Prefab = GetEntity(enemyPrefabDo.prefab, TransformUsageFlags.Dynamic)
                    });
                }
            }
        }
    }
}