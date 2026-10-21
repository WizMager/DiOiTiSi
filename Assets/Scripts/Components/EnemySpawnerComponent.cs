using Unity.Entities;

namespace Components
{
    public struct EnemySpawnerComponent : IBufferElementData
    {
        public int StartIndex;
        public int EndIndex;
        public float SpawnCooldown;
    }
}