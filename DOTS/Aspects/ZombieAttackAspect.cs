using ProjectDawn.Navigation;
using Unity.Entities;

namespace Dungeon
{
    public readonly partial struct ZombieAttackAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<ZombieAttackProperties> _eatProperties;
        private readonly RefRW<ZombieAttackTimer> _timer;
        private readonly RefRO<AgentBody> _agent;
        private readonly RefRO<Hybrid.HealthValue> _mobhealth;

        private int EatDamagePerSecond => _eatProperties.ValueRO.value;
        public float ZombieEatTimer
        {
            get => _timer.ValueRO.value;
            set => _timer.ValueRW.value = value;
        }

        public bool TimeToAttack => ZombieEatTimer <= 0f;

        public float ZombieAttackRate => _eatProperties.ValueRO.cdValue;

        public void Eat(float deltaTime, EntityCommandBuffer.ParallelWriter ecb, int sortKey, Entity brainEntity)
        {
            if (_agent.ValueRO.IsStopped && !_mobhealth.ValueRO.damageTaken)
            {
                var eatDamage = EatDamagePerSecond;
                var curBrainDamage = new Hybrid.PlayerBufferElement { value = eatDamage };
                ecb.AppendToBuffer(sortKey, brainEntity, curBrainDamage);
            }

        }

    }
}