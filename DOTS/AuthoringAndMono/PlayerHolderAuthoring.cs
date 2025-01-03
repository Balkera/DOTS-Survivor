using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Dungeon.Hybrid
{
    public class HolderPrefab : IComponentData
    {
        public GameObject obj;
    }

    public class PlayerHolderReference : ICleanupComponentData
    {
        public Animator animator;
    }
    public struct PlayerHolderPosition : IComponentData
    {
        public LocalTransform playerTransform;
    }
    public struct PlayerHealth : IComponentData
    {
        public int value;
        public int max;
    }
    [InternalBufferCapacity(8)]
    public struct PlayerBufferElement : IBufferElementData
    {
        public int value;
    }
    public class PlayerHolderAuthoring : MonoBehaviour
    {
        public GameObject PlayerGameObjectPrefab;

        public class PlayerHolderBaker : Baker<PlayerHolderAuthoring>
        {
            public override void Bake(PlayerHolderAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponentObject(entity, new HolderPrefab { obj = authoring.PlayerGameObjectPrefab });
                AddComponent(entity, new PlayerHealth { value = 100, max = 100});
                AddComponent<PlayerHolderPosition>(entity);
                AddComponent<PlayerTag>(entity);
                AddBuffer<PlayerBufferElement>(entity);

            }
        }
    }
}