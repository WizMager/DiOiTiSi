using Unity.Entities;

namespace Components
{
    public struct AttackTargetComponent : IComponentData, IEnableableComponent
    {
        public Entity Value;
    }
}