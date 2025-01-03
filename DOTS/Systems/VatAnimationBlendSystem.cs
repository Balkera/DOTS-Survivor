using Unity.Entities;
using Unity.Burst;
using ProjectDawn.Navigation;
using Dungeon.Hybrid;
using Unity.Rendering;

namespace Dungeon
{
    [BurstCompile]
    public partial struct VatAnimationBlendSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (vab, stopOrGo) in
                     SystemAPI.Query<RefRW<VatAnimationBlendFloatOverride>, AgentBody>())
            {
                if (stopOrGo.IsStopped)
                {

                        vab.ValueRW.Value = 1;
                }
                if(!stopOrGo.IsStopped)
                {
                        vab.ValueRW.Value = 0;
                }
            }
            foreach (var (vab, health) in
                     SystemAPI.Query<RefRW<VatAnimationTimeFloatOverride>, RefRO<HealthValue>>())
            {
                if (health.ValueRO.damageTaken)
                {
                    vab.ValueRW.Value = 0;
                }
                else
                {
                    vab.ValueRW.Value = 1;
                }
            }
        }
    }
}

