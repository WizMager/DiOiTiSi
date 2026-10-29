using Enums;
using Unity.Entities;

namespace Components
{
    public struct DropItemsDataBuffer : IBufferElementData
    {
        public EDropItem Type;
        public float Weight;
    }
}