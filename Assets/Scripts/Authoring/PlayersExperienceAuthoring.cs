using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class PlayersExperienceAuthoring : MonoBehaviour
    {
        private class PlayersExperienceAuthoringBaker : Baker<PlayersExperienceAuthoring>
        {
            public override void Bake(PlayersExperienceAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new ExperienceComponent
                {
                    Current = 0,
                    LevelUp = 100
                });
                AddBuffer<GainExperienceBuffer>(entity);
            }
        }
    }
}