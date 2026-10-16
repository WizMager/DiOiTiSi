using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class EnemyAuthoring : MonoBehaviour
    {
        public float heath = 50f;
        public float speed = 10f;
        public float attackCooldown = 1f;
        
        private class EnemyAuthoringBaker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent<EnemyTag>(entity);
                AddBuffer<DamageBuffer>(entity);
                AddComponent(entity, new MoveSpeedComponent
                {
                    Value = authoring.speed
                });
                AddComponent(entity, new HealthComponent
                {
                    Value = authoring.heath
                });
                AddComponent(entity, new AttackCooldownComponent
                {
                    CurrentTime = 0,
                    Cooldown = authoring.attackCooldown
                });
            }
        }
    }
}