using Unity.Entities;
using UnityEngine;

namespace Components
{
    public struct DropItemMaterialsBuffer : IBufferElementData
    {
        public UnityObjectRef<Material> Value;
    }
}