using Unity.Entities;

namespace Components
{
    public struct EnemySpawnCooldownComponent : IComponentData
    {
        public float Cooldown;
        public float CurrentTime;
    }
}