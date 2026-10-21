using Unity.Entities;

namespace Components
{
    public struct EnemySpawnDistanceComponent : IComponentData
    {
        public float LeftRightDistance;
        public float TopBottomDistance;
    }
}