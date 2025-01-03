using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Dungeon
{
    public struct GraveyardProperties : IComponentData
    {
        public float2 FieldDimensions;
        public Entity TombstonePrefab;
        public Entity ZombieType1, ZombieType2, ZombieType3;
        public float ZombieSpawnRate;
        public int NumberTombstonesToSpawn;
    }
    public class ExitPrefab : IComponentData
    {
        public GameObject obj;
    }
    public struct ZombieSpawnTimer : IComponentData
    {
        public float value;
    }
    public struct ZombieCount : IComponentData
    {
        public int value;
        public int valueIncrement;
        public int WaveCountvalue;
        public int WaveCount;
        public int WaveIndex;
        public int IndexCounter;
        public int maxSpawnValue;
    }
    public struct ActiveZombieCount : IComponentData
    {
        public int value;

    }
}
