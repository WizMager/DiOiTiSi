using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct RandomComponent : IComponentData, IEnableableComponent
    {
        public Random Value;
    }
}