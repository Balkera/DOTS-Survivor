using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Dungeon.Hybrid
{
    //[UpdateBefore(typeof(ZombieWalkSystem))]
    public partial struct PlayerHolderSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (playerGameObjectPrefab, entity) in
                     SystemAPI.Query<HolderPrefab>().WithNone<PlayerHolderReference>().WithEntityAccess())
            {
                var newCompanionGameObject = Object.Instantiate(playerGameObjectPrefab.obj);
                var newAnimatorReference = new PlayerHolderReference
                {
                    animator = newCompanionGameObject.GetComponent<Animator>()
                };
                ecb.AddComponent(entity, newAnimatorReference);
            }

            foreach (var (transform, animatorReference, otherTransform) in
                     SystemAPI.Query<RefRW<LocalTransform>, PlayerHolderReference, RefRW<PlayerHolderPosition>>())
            {
                transform.ValueRW.Position = animatorReference.animator.transform.position;
                otherTransform.ValueRW.playerTransform.Position = transform.ValueRO.Position;
            }

           // foreach (var (hp, animatorReference, entity) in
           //SystemAPI.Query<PlayerHealth, PlayerHolderReference>().WithEntityAccess())
           // {
           //     if (hp.value <= 0)
           //     {
           //         ecb.RemoveComponent<PlayerTag>(entity);
           //         var graveyardEntity = SystemAPI.GetSingletonEntity<GraveyardProperties>();

           //         ecb.AddComponent<GraveTag>(graveyardEntity);
           //         ecb.AddComponent<ExitTag>(graveyardEntity);
           //         //Object.Destroy(animatorReference.animator.gameObject);
           //         //ecb.DestroyEntity(entity);
           //         SceneManager.LoadScene("UnderWaterScene");
           //     }
           // }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}