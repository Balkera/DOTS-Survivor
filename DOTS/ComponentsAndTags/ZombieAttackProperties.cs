using Unity.Entities;

public struct ZombieAttackProperties : IComponentData
{
    public int value;
    public float cdValue;
}
public struct ZombieAttackTimer : IComponentData
{
    public float value;
}
