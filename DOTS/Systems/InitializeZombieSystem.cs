//using Unity.Entities;
//using Unity.Burst;
//using Unity.Collections;
//using ProjectDawn.Navigation;


//namespace Dungeon
//{
//    [BurstCompile]
//    [UpdateInGroup(typeof(InitializationSystemGroup))]
//    public partial struct InitializeZombieSystem : ISystem
//    {
//        [BurstCompile]
//        public void OnUpdate(ref SystemState state)
//        {
//            var ecb = new EntityCommandBuffer(Allocator.Temp);
//            foreach (var (zombie, body) in SystemAPI.Query<ZombieAttackAspect, RefRW<AgentBody>> ().WithAll<NewZombieTag>())
//            {
//                //ecb.RemoveComponent<NewZombieTag>(zombie.Entity);
//                body.ValueRW.IsStopped = false;
//                //ecb.SetComponentEnabled<ZombieWalkProperties>(zombie.Entity, true);
//            }
//            ecb.Playback(state.EntityManager);
//        }
//    }
//}
