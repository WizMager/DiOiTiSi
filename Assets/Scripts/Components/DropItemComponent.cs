using Enums;
using Unity.Entities;

namespace Components
{
    public struct DropItemComponent : IComponentData
    {
        public EDropItem ItemType;
        public Entity Icon;
    }
}