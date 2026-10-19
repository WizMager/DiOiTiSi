using System;
using System.Collections.Generic;
using Data.DataObject;
using Enums;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = nameof(EnemySpawnData), menuName = "Parameters/" + nameof(EnemySpawnData))]
    public class EnemySpawnData : ScriptableObject
    {
        [SerializeField] private float _singleEnemySpawnCooldown;
        [SerializeField] private float _enemySpawnerSpawnCooldown;
        [SerializeField] private List<EnemyPrefabDo> _enemies;

        public float SingleEnemySpawnCooldown => _singleEnemySpawnCooldown;
        public float EnemySpawnerSpawnCooldown => _enemySpawnerSpawnCooldown;

        public GameObject GetEnemy(EEnemyType enemyType)
        {
            foreach (var enemyPrefabDo in _enemies)
            {
                if (enemyPrefabDo.enemyType != enemyType)
                    continue;

                return enemyPrefabDo.prefab;
            }

            throw new Exception($"[{nameof(EnemySpawnData)}]: There is not value with key {enemyType}");
        }
    }
}