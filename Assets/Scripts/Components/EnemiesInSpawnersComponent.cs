using Unity.Entities;

namespace Components
{
    public struct EnemiesInSpawnersComponent : IBufferElementData
    {
        public float Weight;
        public Entity Prefab;
    }
}