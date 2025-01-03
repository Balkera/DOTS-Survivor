using ProjectDawn.Navigation;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Dungeon
{
    public readonly partial struct ZombieWalkAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRO<FollowerProperties> _follower;
        private readonly RefRW<AgentBody> _agent;
        private readonly RefRO<AgentSteering> _steer;
        private readonly RefRW<Hybrid.HealthValue> _health;
        private readonly RefRW<Hybrid.DamageEffects> _damageEffects;

        public float3 Follower => _follower.ValueRO.value;
        public float KnockBackCoefficient => _damageEffects.ValueRO.knockBackCoefficient;
        public int KnockBackThreshold => _damageEffects.ValueRO.knockBackThreshold;
        public int StunThreshold => _damageEffects.ValueRO.stunThreshold;


        public void Walk(float deltaTime/*, bool stop*/)
        {

            if (!_health.ValueRO.damageTaken || (StunThreshold == 0 && KnockBackThreshold == 0))
            {
                if (math.distance(_transform.ValueRO.Position, Follower) > _steer.ValueRO.StoppingDistance)
                {
                    _agent.ValueRW.Destination = Follower;
                    _agent.ValueRW.IsStopped = false;
                }

                else
                {
                    _agent.ValueRW.IsStopped = true;

                }
                float3 direction = Follower - _transform.ValueRO.Position;

                quaternion rotation = quaternion.LookRotation(direction, _transform.ValueRO.Up());
                _transform.ValueRW.Rotation = rotation;
            }
            else
            {
                if (KnockBackable())
                {
                    float3 direction = -math.normalize(Follower - _transform.ValueRO.Position);
                    _transform.ValueRW.Position += KnockBackCoefficient * direction * deltaTime;
                    DamageTimer(deltaTime);
                }
                if (Stunable())
                {
                    DamageTimer(deltaTime);
                }
            }
        }
        public bool KnockBackable()
        {
            bool thresholdPassed = false;
            if (KnockBackThreshold > 0 && _health.ValueRO.damageAmount >= KnockBackThreshold)
            {
                thresholdPassed = true;
            }
            return thresholdPassed;
        }
        public bool Stunable()
        {
            bool thresholdPassed = false;
            if (StunThreshold > 0 && _health.ValueRO.damageAmount >= StunThreshold)
            {
                thresholdPassed = true;
            }
            return thresholdPassed;
        }
        public void DamageTimer(float deltaTime)
        {
            if (_damageEffects.ValueRO.effectTimer > _damageEffects.ValueRO.timer)
            {
                _damageEffects.ValueRW.timer += deltaTime;
                _agent.ValueRW.IsStopped = true;

            }
            else
            {
                _damageEffects.ValueRW.timer = 0;
                _agent.ValueRW.IsStopped = false;
                _health.ValueRW.damageTaken = false;
                _health.ValueRW.damageAmount = 0;
            }

        }
        public bool IsInStoppingRange(float3 targetPosition, float stopRadiusSq)
        {
            return math.distancesq(targetPosition, _transform.ValueRO.Position) <= stopRadiusSq;
        }
    }
}