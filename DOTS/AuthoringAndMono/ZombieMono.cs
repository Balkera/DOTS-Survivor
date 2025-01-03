using Unity.Entities;
using UnityEngine;

namespace Dungeon
{
    public class ZombieMono : MonoBehaviour
    {
        public float RiseRate;
        //public float WalkSpeed;
        public int attackDamage;
        public float attackCooldown;
    }

    public class ZombieBaker : Baker<ZombieMono>
    {
        public override void Bake(ZombieMono authoring)
        {
            var zombieEntity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(zombieEntity, new ZombieRiseRate { Value = authoring.RiseRate });
            //AddComponent(zombieEntity, new ZombieWalkProperties { WalkSpeed = authoring.WalkSpeed});
            AddComponent(zombieEntity, new ZombieAttackProperties { value = authoring.attackDamage, cdValue = authoring.attackCooldown});

            AddComponent<ZombieHeading>(zombieEntity);
            AddComponent<NewZombieTag>(zombieEntity);
            AddComponent<FollowerProperties>(zombieEntity);
            //AddComponent<StopOrWalk>(zombieEntity);
            AddComponent<ZombieAttackTimer>(zombieEntity);

        }
    }
}

