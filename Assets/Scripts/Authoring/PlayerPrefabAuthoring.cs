using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class PlayerPrefabAuthoring : MonoBehaviour
    {
        public GameObject playerPrefab;
        
        private class PlayerPrefabAuthoringBaker : Baker<PlayerPrefabAuthoring>
        {
            public override void Bake(PlayerPrefabAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new PlayerPrefabComponent
                {
                    Value = GetEntity(authoring.playerPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}