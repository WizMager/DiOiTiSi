using Gameplay.Components;
using Unity.Entities;
using UnityEngine;

namespace Gameplay.Authoring
{
    public class PlayerPrefabAuthoring : MonoBehaviour
    {
        public GameObject PlayerPrefab;
        
        private class PrefabsAuthoringBaker : Baker<PlayerPrefabAuthoring>
        {
            public override void Bake(PlayerPrefabAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new PlayerPrefabComponent
                {
                    Value = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}