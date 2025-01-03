using Unity.Burst;
using Unity.Entities;

namespace Dungeon
{
    [BurstCompile]
    public partial struct SpawnZombieSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ZombieSpawnTimer>();
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {

            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

            new SpawnZombieJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
            }.Schedule();
        }
    }

    [BurstCompile]
    public partial struct SpawnZombieJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer ECB;

        [BurstCompile]
        private void Execute(GraveyardAspect graveyard)
        {
            graveyard.ZombieSpawnTimer -= DeltaTime;

            if (!graveyard.TimeToSpawnZombie) return;
            if (!graveyard.ZombieSpawnPointInitialized()) return;

            if (!graveyard.NextWaveSpawnable() && graveyard.NoZombiesActive())
            {
                //UnityEngine.Debug.Log("ANAN");
                ECB.RemoveComponent<ZombieSpawnTimer>(graveyard.Entity);
                return;
            }
            if (graveyard.NextWaveSpawnable() && graveyard.NoZombiesActive())
            {
                graveyard.WaveIndex();
                graveyard.WaveNumberLeft--;
                graveyard.MaxZombieToSpawn = graveyard.NewMaxZombieCount;
                graveyard.NumberZombiesToSpawn = graveyard.MaxZombieToSpawn;
                graveyard.ZombieActive = graveyard.MaxZombieToSpawn;
            }

            if (!graveyard.ZombiesSpawnable())
            {
                return;
            }
            if (graveyard.ZombiesSpawnable())
            {
                graveyard.NumberZombiesToSpawn--;
            }
            graveyard.ZombieSpawnTimer = graveyard.ZombieSpawnRate;
            var newZombie = ECB.Instantiate(graveyard.ZombiePrefab());

            var newZombieTransform = graveyard.GetZombieSpawnPoint();
            ECB.SetComponent(newZombie, newZombieTransform);


            var zombieHeading = MathHelpers.GetHeading(newZombieTransform.Position, graveyard.Position);
            ECB.SetComponent(newZombie, new ZombieHeading { value = zombieHeading });





        }
    }
}
