using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class RandomizerAuthoring : MonoBehaviour
    {
        private class RandomizerAuthoringBaker : Baker<RandomizerAuthoring>
        {
            public override void Bake(RandomizerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent<RandomizerTag>(entity);
                AddComponent<RandomComponent>(entity);
                SetComponentEnabled<RandomComponent>(entity, false);
            }
        }
    }
}