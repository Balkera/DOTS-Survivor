using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Dungeon.Hybrid
{
    [UpdateInGroup(typeof(PresentationSystemGroup), OrderFirst = true)]
    public partial struct EnemyAnimateSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (EnemyGameObjectPrefab, entity) in
                     SystemAPI.Query<EnemyGameObjectPrefab>().WithNone<EnemyAnimatorReference>().WithEntityAccess())
            {
                var newCompanionGameObject = Object.Instantiate(EnemyGameObjectPrefab.Value);
                var newAnimatorReference = new EnemyAnimatorReference
                {
                    Value = newCompanionGameObject.GetComponent<Animator>()
                };

                ecb.AddComponent(entity, newAnimatorReference);
            }


            foreach (var (health, effect, EnemyAnimator) in SystemAPI.Query<RefRW<HealthValue>, ZombieWalkAspect, EnemyAnimatorReference>())
            {
                health.ValueRW.health = EnemyAnimator.Value.gameObject.GetComponent<PooledMob>().health;
                if (health.ValueRO.health < health.ValueRO.initHealth)
                {
                    health.ValueRW.damageAmount = health.ValueRO.initHealth - health.ValueRO.health;
                    health.ValueRW.initHealth = health.ValueRO.health;
                    if (effect.Stunable() || effect.KnockBackable())
                    {
                        health.ValueRW.damageTaken = true;

                    }
                }
                else
                {
                    health.ValueRW.initHealth = health.ValueRO.health;
                }
            }
            foreach (var (transform, animatorReference) in
                SystemAPI.Query<LocalTransform, EnemyAnimatorReference>())
            {
                animatorReference.Value.transform.SetPositionAndRotation(transform.Position, transform.Rotation);
            }
            foreach (var (animatorReference, hp, entity) in
                     SystemAPI.Query<EnemyAnimatorReference, HealthValue>().WithEntityAccess())
            {
                if (hp.health <= 0)
                {
                    var graveyardEntity = SystemAPI.GetSingletonEntity<GraveyardProperties>();
                    SystemAPI.GetComponentRW<ActiveZombieCount>(graveyardEntity).ValueRW.value -= 1;

                    Object.Destroy(animatorReference.Value.gameObject);
                    ecb.DestroyEntity(entity);
                }
                // ecb.RemoveComponent<EnemyAnimatorReference>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}