using Unity.Entities;
using UnityEngine;

namespace Dungeon
{
    
    public class GraveTagAuthoring : MonoBehaviour
    {

        public class PlayerHolderBaker : Baker<GraveTagAuthoring>
        {
            public override void Bake(GraveTagAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent<GraveTag>(entity);
                AddComponent<ExitTag>(entity);

            }
        }
    }
}