using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct SpawnDropItemPositionsBuffer : IBufferElementData
    {
        public float3 Value;
    }
}