using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class GameplayAuthoring : MonoBehaviour
    {
        private class GameplayAuthoringBaker : Baker<GameplayAuthoring>
        {
            public override void Bake(GameplayAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent<LevelUpComponent>(entity);
                AddComponent<ContinueGameTag>(entity);
                SetComponentEnabled<ContinueGameTag>(entity, false);
                AddComponent<IsPauseTag>(entity);
                SetComponentEnabled<IsPauseTag>( entity, false);
            }
        }
    }
}