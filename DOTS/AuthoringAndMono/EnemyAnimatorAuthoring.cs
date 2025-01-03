using Unity.Entities;
using UnityEngine;

namespace Dungeon.Hybrid
{
    public class EnemyGameObjectPrefab : IComponentData
    {
        public GameObject Value;
    }

    public class EnemyAnimatorReference : ICleanupComponentData
    {
        public Animator Value;
    }
    public struct HealthValue : IComponentData
    {
        public int health;
        public int initHealth;
        public bool damageTaken;
        public int damageAmount;
    }
    public struct DamageEffects : IComponentData
    {
        public float timer;
        public float effectTimer;
        public float knockBackCoefficient;
        public int stunThreshold;
        public int knockBackThreshold;
    }

    public class EnemyAnimatorAuthoring : MonoBehaviour
    {
        public GameObject EnemyGameObjectPrefab;
        public float effectTimer;
        public float knockBackCoefficient;
        public int stunThreshold;
        public int knockBackThreshold;

        public class EnemyGameObjectPrefabBaker : Baker<EnemyAnimatorAuthoring>
        {
            public override void Bake(EnemyAnimatorAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponentObject(entity, new EnemyGameObjectPrefab { Value = authoring.EnemyGameObjectPrefab });
                AddComponent(entity, new DamageEffects
                {
                    effectTimer = authoring.effectTimer,
                    timer = 0,
                    stunThreshold  = authoring.stunThreshold,
                    knockBackThreshold = authoring.knockBackThreshold,
                    knockBackCoefficient = authoring.knockBackCoefficient,
                });
                AddComponent<HealthValue>(entity);
            }
        }
    }
}