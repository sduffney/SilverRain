using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Health : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float maxHP = 50f;
    public float currentHP = 50f;
    public float armor = 0f;

    public System.Action<DamagePayload, float> OnDamaged; // applied amount
    public System.Action OnDeath;

    public float ApplyDamage(DamagePayload payload)
    {
        float applied = Mathf.Max(0f, payload.rawDamage - armor);
        currentHP -= applied;
        OnDamaged?.Invoke(payload, applied);

        if (currentHP <= 0f) OnDeath?.Invoke();
        return applied;
    }

    public Vector3 GetWorldPosition() => transform.position;
}

// ***THIS IS NOT PART OF THIS CODE, NEEDS TO BE IMPLEMENTED IN A SEPARATE FILE***
/* 
 //EnemyController (setup)
 // Hook invisibility and blood splatter in Shane’s system by subscribing to OnDamaged during enemy setup
void Awake()
{
    var health = GetComponent<Health>();
    health.OnDamaged += (payload, applied) =>
    {
        // Reveal and splatter
        EnemyInvisibility.Reveal(this, duration: 2f);
        BloodSplatter.Spawn(health.GetWorldPosition());
        // Optional: notify progression (Xiaoyu) or UI hit indicator (Zibing)
    };

    health.OnDeath += () =>
    {
        ProgressionRewards.GrantOnKill(this); // XP/Gold
        EnemyControllerManager.Instance.Despawn(this);
    };
}*/