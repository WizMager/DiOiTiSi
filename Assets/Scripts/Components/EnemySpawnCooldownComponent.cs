using Unity.Entities;

namespace Components
{
    public struct EnemySpawnCooldownComponent : IComponentData
    {
        public float Value;
    }
}