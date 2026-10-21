using Components;
using Data;
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
                
                AddComponent(entity, new EnemySpawnCooldownComponent
                {
                    Cooldown = authoring.enemySpawnData.SingleEnemySpawnCooldown,
                    CurrentTime = authoring.enemySpawnData.SingleEnemySpawnCooldown
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
                SetComponentEnabled<CanSpawnTag>(entity, false);
                
                var singleEnemies = AddBuffer<EnemyPrefabBuffer>(entity);
                foreach (var enemyPrefabDo in authoring.enemySpawnData.Enemies)
                {
                    if (!enemyPrefabDo.prefab)
                        continue;
                    
                    singleEnemies.Add(new EnemyPrefabBuffer
                    {
                        Weight = enemyPrefabDo.weight,
                        Prefab = GetEntity(enemyPrefabDo.prefab, TransformUsageFlags.Dynamic),
                    });
                }

                //Spawner of spawners
                var spawnerOfSpawnersEntity = CreateAdditionalEntity(TransformUsageFlags.None);
                
                AddComponent(spawnerOfSpawnersEntity, new EnemySpawnCooldownComponent
                {
                    Cooldown = authoring.enemySpawnData.EnemySpawnerSpawnCooldown,
                    CurrentTime = authoring.enemySpawnData.EnemySpawnerSpawnCooldown
                });
                AddComponent(spawnerOfSpawnersEntity, new RandomComponent
                {
                    Value = Random.CreateFromIndex((uint)spawnerOfSpawnersEntity.Index + 1),
                });
                AddComponent<SpawnerOfSpawnerTag>(spawnerOfSpawnersEntity);
                AddComponent<CanSpawnTag>(spawnerOfSpawnersEntity);
                SetComponentEnabled<CanSpawnTag>(spawnerOfSpawnersEntity, false);
                AddComponent(spawnerOfSpawnersEntity, new EnemySpawnDistanceComponent
                {
                    LeftRightDistance = authoring.enemySpawnData.SpawnerLeftRightDistance,
                    TopBottomDistance = authoring.enemySpawnData.SpawnerTopBottomDistance
                });
                
                var spawners = AddBuffer<EnemyPrefabBuffer>(spawnerOfSpawnersEntity);
                foreach (var enemyPrefabDo in authoring.enemySpawnData.EnemySpawners)
                {
                    if (!enemyPrefabDo.prefab)
                        continue;
                    
                    spawners.Add(new EnemyPrefabBuffer
                    {
                        Weight =  enemyPrefabDo.weight,
                        Prefab = GetEntity(enemyPrefabDo.prefab, TransformUsageFlags.Dynamic),
                    });
                }
            }
        }
    }
}