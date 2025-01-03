using Unity.Burst;
using Unity.Entities;

namespace Dungeon
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial struct ZombieAttackSystem : ISystem
    {
        public float timer;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
            timer = 0;
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            new ZombieAttackJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                BrainEntity = playerEntity,
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ZombieAttackJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;
        public Entity BrainEntity;

        [BurstCompile]
        private void Execute(ZombieAttackAspect zombie, [ChunkIndexInQuery] int sortKey)
        {
            zombie.ZombieEatTimer -= DeltaTime;
            if (!zombie.TimeToAttack) return;
            zombie.Eat(DeltaTime, ECB, sortKey, BrainEntity);
            zombie.ZombieEatTimer = zombie.ZombieAttackRate;

        }
    }

}
