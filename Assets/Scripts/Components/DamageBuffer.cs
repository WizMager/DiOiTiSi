using Unity.Entities;

namespace Components
{
    [InternalBufferCapacity(8)]
    public struct DamageBuffer : IBufferElementData
    {
        public float Value;
    }
}