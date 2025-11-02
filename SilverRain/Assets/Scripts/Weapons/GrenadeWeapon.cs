using UnityEngine;

public class GrenadeWeapon : TemporaryWeapon
{
    public Transform firePoint;
    public GameObject grenadePrefab;

    public override void Attack()
    {
        if (!IsOffCooldown()) return;

        var grenadeObj = Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);
        var rb = grenadeObj.GetComponent<Rigidbody>();

        // Launch with throwAngle
        Vector3 launchDir = Quaternion.AngleAxis(throwAngle, firePoint.right) * firePoint.forward;
        rb.linearVelocity = launchDir * projectileSpeed;

        // Pass damage + explosion radius
        var grenade = grenadeObj.GetComponent<Grenade>();
        grenade.Init(GetDamage(), baseSize);

        ResetCooldown();
    }
}

public class Grenade : MonoBehaviour
{
    private float damage;
    private float radius;

    public void Init(float dmg, float aoeRadius)
    {
        damage = dmg;
        radius = aoeRadius;
        Invoke(nameof(Explode), 2f); // fixed delay for now
    }

    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var c in hits)
        {
            //var dmgTarget = c.GetComponent<IDamageable>();
            //if (dmgTarget != null)
            //    dmgTarget.ApplyDamage(new DamagePayload { rawDamage = damage, instigator = gameObject });
        }
        Destroy(gameObject);
    }
}