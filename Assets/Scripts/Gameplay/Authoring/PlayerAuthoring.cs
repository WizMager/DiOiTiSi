using Gameplay.Components;
using Unity.Entities;
using UnityEngine;

namespace Gameplay.Authoring
{
    public class PlayerAuthoring : MonoBehaviour
    {
        private class PlayerAuthoringBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent<PlayerTag>(entity);
            }
        }
    }
}