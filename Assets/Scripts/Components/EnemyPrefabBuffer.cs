using Unity.Entities;

namespace Components
{
    public struct EnemyPrefabBuffer : IBufferElementData
    {
        public float Weight;
        public Entity Prefab;
    }
}