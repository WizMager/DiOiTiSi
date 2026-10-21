using System;
using UnityEngine;

namespace Data.DataObject
{
    [Serializable]
    public struct EnemyInSpawnerDo
    {
        public float weight;
        public GameObject enemyPrefab;
    }
}