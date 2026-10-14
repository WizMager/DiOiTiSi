using Unity.Entities;
using UnityEngine;

namespace Components
{
    public struct CameraComponent : IComponentData
    {
        public UnityObjectRef<Camera> Value;
    }
}