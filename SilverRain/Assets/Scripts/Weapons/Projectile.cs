using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;
    private Vector3 direction;
    private float speed;
    private bool isExplosive;
    private float aoeRadius;

    public void Init(float dmg, Vector3 dir, float spd, bool isExplosive = false, float aoeRadius = 0f)
    {
        damage = dmg;
        direction = dir.normalized;
        speed = spd;
        this.isExplosive = isExplosive;
        this.aoeRadius = aoeRadius;
    }

    void Update() =>
        transform.position += direction * speed * Time.deltaTime;

    void OnCollisionEnter(Collision col)
    {
        if (isExplosive)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, aoeRadius);
            foreach (var h in hits)
                h.GetComponent<IDamageable>()?.ApplyDamage(new DamagePayload { rawDamage = damage, instigator = gameObject });
        }
        else
        {
            col.collider.GetComponent<IDamageable>()?.ApplyDamage(new DamagePayload { rawDamage = damage, instigator = gameObject });
        }

        Destroy(gameObject);
    }
}