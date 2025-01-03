using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Dungeon
{
    public class GraveyardMono : MonoBehaviour
    {
        public float2 FieldDimensions;
        public int NumberTombstonesToSpawn;
        public GameObject TombstonePrefab;
        public uint RandomSeed;
        // public GameObject ZombiePrefab;
        public GameObject ZombieType1, ZombieType2, ZombieType3;
        public float ZombieSpawnRate;
        public int ZombieCount, zombieCountIncrement;
        public int WaveCount;
        public GameObject ExitPrefab;
        public float zombieScale;
    }

    public class GraveyardBaker : Baker<GraveyardMono>
    {
        public override void Bake(GraveyardMono authoring)
        {
            var graveyardEntity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(graveyardEntity, new GraveyardProperties
            {
                FieldDimensions = authoring.FieldDimensions,
                NumberTombstonesToSpawn = authoring.NumberTombstonesToSpawn,
                TombstonePrefab = GetEntity(authoring.TombstonePrefab, TransformUsageFlags.Dynamic),
                //ZombiePrefab = GetEntity(authoring.ZombiePrefab, TransformUsageFlags.Dynamic),
                ZombieType1 = GetEntity(authoring.ZombieType1, TransformUsageFlags.Dynamic),
                ZombieType2 = GetEntity(authoring.ZombieType2, TransformUsageFlags.Dynamic),
                ZombieType3 = GetEntity(authoring.ZombieType3, TransformUsageFlags.Dynamic),
                ZombieSpawnRate = authoring.ZombieSpawnRate,
            });
            AddComponent(graveyardEntity, new GraveyardRandom
            {
                value = Random.CreateFromIndex(authoring.RandomSeed)
            });
            AddComponent(graveyardEntity, new ZombieCount
            {
                value = authoring.ZombieCount,
                valueIncrement = authoring.zombieCountIncrement,
                WaveIndex = 1,
                IndexCounter = 1,
                maxSpawnValue = authoring.ZombieCount,
                WaveCountvalue = authoring.WaveCount,
                WaveCount = authoring.WaveCount,
            });
            AddComponent(graveyardEntity, new ZombieScale
            {
                Value = authoring.zombieScale,
            });
            AddComponent(graveyardEntity, new ActiveZombieCount
            {
                value = authoring.ZombieCount,
            });
            AddComponentObject(graveyardEntity, new ExitPrefab
            {
                obj = authoring.ExitPrefab
            });
            AddComponent<ZombieSpawnPoints>(graveyardEntity);
            AddComponent<ZombieSpawnTimer>(graveyardEntity);


        }
    }
}