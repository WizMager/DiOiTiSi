using Unity.Entities;
using UnityEngine.Rendering;

namespace Components
{
    public struct DropItemMaterialsIdBuffer : IBufferElementData
    {
        public BatchMaterialID Value;
    }
}