using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class EnemyAuthoring : MonoBehaviour
    {
        public float heath = 50f;
        public float speed = 12f;
        public float attackCooldown = 1f;
        public float attackDistance = 2f;
        public float damage = 10f;
        public float experienceDrop = 25;
        
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
                AddComponent(entity, new AttackDistanceComponent
                {
                    Value = authoring.attackDistance
                });
                AddComponent<AttackTargetComponent>(entity);
                SetComponentEnabled<AttackTargetComponent>(entity,false);
                AddComponent<CanAttackTag>(entity);
                AddComponent(entity, new DamageComponent
                {
                    Value = authoring.damage
                });
                AddComponent(entity, new ExperienceDropComponent
                {
                    Value = authoring.experienceDrop
                });
            }
        }
    }
}