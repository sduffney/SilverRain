//using UnityEngine;

//public class SwordWeapon : TempWeapon
//{
//    public LayerMask hitMask;

//    public override void Attack()
//    {
//        if (!IsOffCooldown()) return;

//        Vector3 origin = transform.position;
//        Vector3 forward = transform.forward;

//        Collider[] hits = Physics.OverlapSphere(origin + forward * (stats.reach * 0.5f), stats.reach, hitMask);
//        foreach (var c in hits)
//        {
//            Vector3 toTarget = (c.transform.position - origin).normalized;
//            if (Vector3.Angle(forward, toTarget) <= stats.arcDegrees * 0.5f)
//            {
//                var dmg = c.GetComponent<IDamageable>();
//                if (dmg != null)
//                    dmg.ApplyDamage(new DamagePayload { rawDamage = GetDamage(), instigator = gameObject });
//            }
//        }

//        ResetCooldown();
//    }
//}

using UnityEngine;

public class SwordWeapon : TemporaryWeapon
{
    public Transform player;
    public GameObject swordPrefab;
    public LayerMask hitMask;

    private GameObject swordInstance;
    private float activeUntil = -1f; // when the sword stops spinning
    private float angle;

    private void Update()
    {
        if (swordInstance == null) return;

        // If duration expired, disable sword
        if (baseDuration > 0 && Time.time > activeUntil)
        {
            Destroy(swordInstance);
            swordInstance = null;
            return;
        }

        // Orbit logic
        angle += 180f * Time.deltaTime; // could scale with level
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * baseSize;
        swordInstance.transform.position = player.position + offset;
        swordInstance.transform.LookAt(player.position);
    }

    public override void Attack()
    {
        if (!IsOffCooldown()) return;

        // Spawn sword if not already active
        if (swordInstance == null)
        {
            swordInstance = Instantiate(swordPrefab, player.position, Quaternion.identity);
        }

        // Set active duration
        if (baseDuration > 0)
            activeUntil = Time.time + baseDuration;

        ResetCooldown();
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(Mathf.RoundToInt(GetDamage()));
        }
    }
}