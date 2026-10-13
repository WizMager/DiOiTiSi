using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct InputComponent : IComponentData
    {
        public float2 MoveDirection;
        public float2 CrosshairPosition;
        public bool Attack;
        public bool LevelUpMenu;
    }
}