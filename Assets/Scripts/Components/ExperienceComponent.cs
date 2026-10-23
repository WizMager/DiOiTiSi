using Unity.Entities;

namespace Components
{
    public struct ExperienceComponent : IComponentData
    {
        public float Current;
        public float LevelUp;
    }
}