using Unity.Entities;

namespace Dungeon
{
    public readonly partial struct PlayerAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<Hybrid.PlayerHealth> _Health;
        private readonly DynamicBuffer<Hybrid.PlayerBufferElement> _PlayerDamageBuffer;

        public void DamagePlayer()
        {
            foreach (var brainDamageBufferElement in _PlayerDamageBuffer)
            {
                _Health.ValueRW.value -= brainDamageBufferElement.value;
            }
            _PlayerDamageBuffer.Clear();

        }
    }
}