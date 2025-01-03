using Unity.Entities;

namespace Unity.Rendering
{
    [MaterialProperty("_VatAnimationBlend")]
    struct VatAnimationBlendFloatOverride : IComponentData
    {
        public float Value;
    }
}
