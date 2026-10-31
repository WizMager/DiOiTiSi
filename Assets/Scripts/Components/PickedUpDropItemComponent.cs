using Enums;
using Unity.Entities;

namespace Components
{
    public struct PickedUpDropItemComponent : IBufferElementData
    {
        public EDropItem ItemType;
    }
}