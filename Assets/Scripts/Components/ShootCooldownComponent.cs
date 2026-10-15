using Unity.Entities;

namespace Components
{
    public struct ShootCooldownComponent : IComponentData
    {
        public float CurrentTime;
        public float Cooldown;
    }
}