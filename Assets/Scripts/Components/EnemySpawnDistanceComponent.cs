using Unity.Entities;

namespace Components
{
    public struct EnemySpawnDistanceComponent : IComponentData
    {
        public float SingleEnemyLeftRightDistance;
        public float SingleEnemyTopBottomDistance;
        public float SpawnerLeftRightDistance;
        public float SpawnerTopBottomDistance;
    }
}