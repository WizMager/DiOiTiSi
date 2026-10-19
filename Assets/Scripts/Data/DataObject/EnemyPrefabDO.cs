using System;
using Enums;
using UnityEngine;

namespace Data.DataObject
{
    [Serializable]
    public struct EnemyPrefabDo
    {
        public EEnemyType enemyType;
        public GameObject prefab;
    }
}