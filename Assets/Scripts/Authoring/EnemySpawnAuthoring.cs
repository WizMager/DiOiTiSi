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
                DependsOn(authoring.enemySpawnData);
                
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
                    Value = Random.CreateFromIndex((uint)entity.Index + 1),
                });
                AddComponent(entity, new EnemySpawnDistanceComponent
                {
                    LeftRightDistance = authoring.enemySpawnData.SingleEnemyLeftRightDistance,
                    TopBottomDistance = authoring.enemySpawnData.SingleEnemyTopBottomDistance,
                });
                AddComponent<SingleEnemySpawnerTag>(entity);
                AddComponent<CanSpawnTag>(entity);

                var spawnerOfSpawnersEntity = CreateAdditionalEntity(TransformUsageFlags.None);
                
                AddComponent(spawnerOfSpawnersEntity, new EnemySpawnCooldownComponent
                {
                    Cooldown = authoring.enemySpawnData.EnemySpawnerSpawnCooldown,
                    CurrentTime = 0
                });
                AddComponent(spawnerOfSpawnersEntity, new RandomComponent
                {
                    Value = Random.CreateFromIndex((uint)spawnerOfSpawnersEntity.Index + 1),
                });
                AddComponent<SpawnerOfSpawnerTag>(spawnerOfSpawnersEntity);
                AddComponent<CanSpawnTag>(spawnerOfSpawnersEntity);
                AddComponent(spawnerOfSpawnersEntity, new EnemySpawnDistanceComponent
                {
                    LeftRightDistance = authoring.enemySpawnData.SpawnerLeftRightDistance,
                    TopBottomDistance = authoring.enemySpawnData.SpawnerTopBottomDistance
                });
                
                var spawners = AddBuffer<EnemySpawnerComponent>(spawnerOfSpawnersEntity);
                var allEnemies = AddBuffer<EnemiesInSpawnersComponent>(spawnerOfSpawnersEntity);
                
                foreach (var enemySpawnerDo in authoring.enemySpawnData.EnemySpawners)
                {
                    var start = allEnemies.Length;
                    
                    foreach (var enemyInSpawnerDo in enemySpawnerDo.enemies)
                    {
                        allEnemies.Add(new EnemiesInSpawnersComponent
                        {
                            Weight = enemyInSpawnerDo.weight,
                            Prefab = GetEntity(enemyInSpawnerDo.enemyPrefab, TransformUsageFlags.Dynamic)
                        });
                    }

                    if (allEnemies.Length <= start)
                        continue;

                    spawners.Add(new EnemySpawnerComponent
                    {
                        StartIndex = start,
                        EndIndex = allEnemies.Length,
                        SpawnCooldown = enemySpawnerDo.spawnCooldown
                    });
                }
            }
        }
    }
}