using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public float moveSpeed = 10f;
        
        private class PlayerAuthoringBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent<PlayerTag>(entity);
                AddComponent(entity, new MoveSpeedComponent
                {
                    Value = authoring.moveSpeed
                });
                AddComponent<InputComponent>(entity);
            }
        }
    }
}