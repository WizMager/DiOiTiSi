using System.Collections.Generic;
using Data.DataObject;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = nameof(EnemySpawnData), menuName = "Parameters/" + nameof(EnemySpawnData))]
    public class EnemySpawnData : ScriptableObject
    {
        [Header("Enemies:")]
        [SerializeField] private float _singleEnemySpawnCooldown;
        [SerializeField] private List<EnemyPrefabDo> _enemies;
        [SerializeField] private float _singleEnemyLeftRightDistance;
        [SerializeField] private float _singleEnemyTopBottomDistance;
        
        [Space]
        [Header("Spawner:")]
        [SerializeField] private float _enemySpawnerSpawnCooldown;
        [SerializeField] private float _spawnerLeftRightDistance;
        [SerializeField] private float _spawnerTopBottomDistance;
        [SerializeField] private List<EnemyPrefabDo> _spawners;

        public float SingleEnemySpawnCooldown => _singleEnemySpawnCooldown;
        public List<EnemyPrefabDo> Enemies => _enemies;
        public float EnemySpawnerSpawnCooldown => _enemySpawnerSpawnCooldown;
        public float SingleEnemyLeftRightDistance => _singleEnemyLeftRightDistance;
        public float SingleEnemyTopBottomDistance => _singleEnemyTopBottomDistance;
        public float SpawnerLeftRightDistance => _spawnerLeftRightDistance;
        public float SpawnerTopBottomDistance => _spawnerTopBottomDistance;
        public List<EnemyPrefabDo> EnemySpawners => _spawners;
    }
}