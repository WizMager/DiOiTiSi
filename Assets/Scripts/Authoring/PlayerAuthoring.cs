using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public float moveSpeed = 10f;
        public float shootCooldown = 0.5f;
        public float health = 100f;
        
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
                AddComponent(entity, new AttackCooldownComponent
                {
                    CurrentTime = 0,
                    Cooldown = authoring.shootCooldown
                });
                AddBuffer<DamageBuffer>(entity);
                AddComponent(entity, new HealthComponent
                {
                    Value = authoring.health
                });
                AddComponent<CanAttackTag>(entity);
                AddBuffer<PickedUpDropItemComponent>(entity);
            }
        }
    }
}