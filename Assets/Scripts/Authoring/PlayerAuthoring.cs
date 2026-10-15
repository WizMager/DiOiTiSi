using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public float moveSpeed = 10f;
        public float shootCooldown = 0.5f;
        
        public Transform shootPoint;

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
                AddComponent(entity, new ShootPointComponent
                {
                    Value = GetEntity(authoring.shootPoint, TransformUsageFlags.Dynamic)
                });
                AddComponent(entity, new ShootCooldownComponent
                {
                    CurrentTime = 0,
                    Cooldown = authoring.shootCooldown
                });
            }
        }
    }
}