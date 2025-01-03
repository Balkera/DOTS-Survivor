using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
namespace Dungeon
{
    public partial struct DungeonExitSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ExitTag>();
        }
        public void OnUpdate(ref SystemState state)
        {

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (ExitGameObjectPrefab, zombieCount, waveCount) in
                     SystemAPI.Query<ExitPrefab, ActiveZombieCount, ZombieCount>())
            {
                if (zombieCount.value <= 0 && waveCount.WaveCountvalue == 1)
                {
                    var graveyardEntity = SystemAPI.GetSingletonEntity<GraveyardProperties>();

                    var final = Object.Instantiate(ExitGameObjectPrefab.obj, new Vector3(-25, -1, -1.5f), Quaternion.identity);
                    final.transform.localScale = ExitGameObjectPrefab.obj.transform.localScale;
                    ecb.RemoveComponent<ExitTag>(graveyardEntity);

                }
            }

            
            ecb.Playback(state.EntityManager);

        }
    }
}
