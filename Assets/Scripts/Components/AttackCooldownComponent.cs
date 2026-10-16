using Unity.Entities;

namespace Components
{
    public struct AttackCooldownComponent : IComponentData
    {
        public float CurrentTime;
        public float Cooldown;
    }
}