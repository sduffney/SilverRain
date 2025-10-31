using UnityEngine;

public interface IDamageable
{
    float ApplyDamage(DamagePayload payload);
    Vector3 GetWorldPosition();
}

public struct DamagePayload
{
    public float rawDamage;
    public bool isCrit;
    public float knockback;
    public Vector3 hitDirection;
    public GameObject instigator;
}