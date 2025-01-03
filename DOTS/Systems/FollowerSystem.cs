using Unity.Mathematics;
using Unity.Collections;
using Unity.Entities;
using Unity.Burst;

namespace Dungeon
{
    [BurstCompile]
    [UpdateBefore(typeof(Hybrid.PlayerHolderSystem))]
    public partial struct FollowerSystem : ISystem
    {
        private float3 targetPos;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityManager entityManager = state.EntityManager;
            NativeArray<Entity> entities = entityManager.GetAllEntities(Allocator.Temp);
            foreach(Entity entity in entities)
            {
                if (entityManager.HasComponent<Hybrid.PlayerHolderPosition>(entity))
                {
                    Hybrid.PlayerHolderPosition target = entityManager.GetComponentData<Hybrid.PlayerHolderPosition>(entity);
                    targetPos = target.playerTransform.Position;

                }
                if (entityManager.HasComponent<FollowerProperties>(entity))
                {
                    FollowerProperties followerProperties = entityManager.GetComponentData<FollowerProperties>(entity);
                    followerProperties.value = targetPos;
                    entityManager.SetComponentData(entity, followerProperties);
                }
            }

        }
    }

}
