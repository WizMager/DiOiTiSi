using System;
using System.Collections.Generic;

namespace Data.DataObject
{
    [Serializable]
    public struct EnemySpawnerDo
    {
        public float spawnCooldown;
        public List<EnemyInSpawnerDo> enemies;
    }
}