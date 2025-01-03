using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Dungeon
{
    public readonly partial struct GraveyardAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<LocalTransform> _transform;
        private LocalTransform Transform => _transform.ValueRO;

        private readonly RefRO<GraveyardProperties> _graveyardProperties;
        private readonly RefRW<GraveyardRandom> _graveyardRandom;
        private readonly RefRW<ZombieSpawnPoints> _zombieSpawnPoints;
        private readonly RefRW<ZombieSpawnTimer> _zombieSpawnTimer;
        private readonly RefRW<ZombieCount> _zombieCount;
        private readonly RefRO<ZombieScale> _zombieScale;
        private readonly RefRW<ActiveZombieCount> _zombieActive;
        public int NumberTombstonesToSpawn => _graveyardProperties.ValueRO.NumberTombstonesToSpawn;
        public float ZombieScaleValue => _zombieScale.ValueRO.Value;

        public int ZombieActive
        {
            get => _zombieActive.ValueRO.value;
            set => _zombieActive.ValueRW.value = value;
        }
        public int NumberZombiesToSpawn
        {
            get => _zombieCount.ValueRO.value;
            set => _zombieCount.ValueRW.value = value;
        }
        public int WaveNumberLeft
        {
            get => _zombieCount.ValueRO.WaveCountvalue;
            set => _zombieCount.ValueRW.WaveCountvalue = value;
        }
        public int MaxZombieToSpawn
        {
            get => _zombieCount.ValueRO.maxSpawnValue;
            set => _zombieCount.ValueRW.maxSpawnValue = value;
        }
        public Entity TombstonePrefab => _graveyardProperties.ValueRO.TombstonePrefab;

        public bool ZombieSpawnPointInitialized()
        {
            return _zombieSpawnPoints.ValueRO.Value.IsCreated && ZombieSpawnPointCount > 0;
        }

        public bool ZombiesSpawnable()
        {
            return NumberZombiesToSpawn > 0;
        }
        public bool NextWaveSpawnable()
        {
            return WaveNumberLeft > 1;
        }
        public bool NoZombiesActive()
        {
            return _zombieActive.ValueRO.value == 0;
        }
        private int ZombieSpawnPointCount => _zombieSpawnPoints.ValueRO.Value.Value.Value.Length;
        public int NewMaxZombieCount => _zombieCount.ValueRO.valueIncrement + MaxZombieToSpawn;
        public LocalTransform GetRandomTombstoneTransform()
        {
            return new LocalTransform
            {
                Position = GetRandomPosition(),
                Rotation = GetRandomRotation(),
                Scale = GetRandomScale(0.5f)
            };
        }

        private float3 GetRandomPosition()
        {
            float3 randomPosition;
            do
            {
                randomPosition = _graveyardRandom.ValueRW.value.NextFloat3(MinCorner, MaxCorner);
            } while (math.distancesq(Transform.Position, randomPosition) <= BRAIN_SAFETY_RADIUS_SQ);

            return randomPosition;
        }

        private float3 MinCorner => Transform.Position - HalfDimensions;
        private float3 MaxCorner => Transform.Position + HalfDimensions;
        private float3 HalfDimensions => new()
        {
            x = _graveyardProperties.ValueRO.FieldDimensions.x * 0.5f,
            y = 0f,
            z = _graveyardProperties.ValueRO.FieldDimensions.y * 0.5f
        };
        private const float BRAIN_SAFETY_RADIUS_SQ = 100;

        private quaternion GetRandomRotation() => quaternion.RotateY(_graveyardRandom.ValueRW.value.NextFloat(-0.25f, 0.25f));
        private float GetRandomScale(float min) => _graveyardRandom.ValueRW.value.NextFloat(min, 1f);

        public float2 GetRandomOffset()
        {
            return _graveyardRandom.ValueRW.value.NextFloat2();
        }

        public float ZombieSpawnTimer
        {
            get => _zombieSpawnTimer.ValueRO.value;
            set => _zombieSpawnTimer.ValueRW.value = value;
        }

        public bool TimeToSpawnZombie => ZombieSpawnTimer <= 0f;

        public float ZombieSpawnRate => _graveyardProperties.ValueRO.ZombieSpawnRate;
        public float MaxZombieType()
        {
            var typeCount = 1;
            if (_graveyardProperties.ValueRO.ZombieType2 != Entity.Null)
            {
                typeCount = 2;
            }
            if (_graveyardProperties.ValueRO.ZombieType3 != Entity.Null)
            {
                typeCount = 3;
            }
            return typeCount;
        }
        public float WaveDistribution()
        {
            var slice = _zombieCount.ValueRO.WaveCount / MaxZombieType();
            return slice;
        }
        public int WaveIndex()
        {
            if (_zombieCount.ValueRW.IndexCounter <= WaveDistribution())
            {
                _zombieCount.ValueRW.IndexCounter++;
            }
            if (_zombieCount.ValueRO.IndexCounter > WaveDistribution() && _zombieCount.ValueRO.WaveIndex <= MaxZombieType())
            {
                _zombieCount.ValueRW.IndexCounter = 1;
                _zombieCount.ValueRW.WaveIndex++;
            }

            return _zombieCount.ValueRO.WaveIndex;

        }
        public Entity ChooseZombieType()
        {
            Entity chosen = _graveyardProperties.ValueRO.ZombieType1;

            if (_zombieCount.ValueRO.WaveIndex == 2)
                chosen = _graveyardProperties.ValueRO.ZombieType2;
            if (_zombieCount.ValueRO.WaveIndex == 3)
                chosen = _graveyardProperties.ValueRO.ZombieType3;

            return chosen;

        }
        public Entity ZombiePrefab()
        {
            MaxZombieType();
            // TypeOverWave();
            // WaveIndex();
            return ChooseZombieType();
        }

        public LocalTransform GetZombieSpawnPoint()
        {
            var position = GetRandomZombieSpawnPoint();
            return new LocalTransform
            {
                Position = position,
                Rotation = quaternion.RotateZ(0),
                Scale = ZombieScaleValue
            };
        }

        private float3 GetRandomZombieSpawnPoint()
        {
            return GetZombieSpawnPoint(_graveyardRandom.ValueRW.value.NextInt(ZombieSpawnPointCount));
        }

        private float3 GetZombieSpawnPoint(int i) => _zombieSpawnPoints.ValueRO.Value.Value.Value[i];

        public float3 Position => Transform.Position;
    }
}