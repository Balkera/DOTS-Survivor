using Unity.Entities;

namespace Dungeon
{
//public struct ZombieWalkProperties : IComponentData, IEnableableComponent
//    {
//        public float WalkSpeed;
//    }
    public struct ZombieTimer : IComponentData
    {
        public float value;
    }
    public struct ZombieHeading : IComponentData
    {
        public float value;
    }
    public struct NewZombieTag : IComponentData
    {
    }
}


