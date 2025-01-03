using Unity.Entities;
using Unity.Mathematics;


namespace Dungeon
{
    public struct FollowerProperties : IComponentData
    {
        public float3 value;
        public int health;
    }
    public struct StopOrWalk : IComponentData
    {
        public bool stop;
    }
}



