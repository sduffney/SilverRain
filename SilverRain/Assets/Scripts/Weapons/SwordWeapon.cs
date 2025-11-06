using UnityEngine;

public class SwordWeapon : TempWeapon
{
    public LayerMask hitMask;
    public GameObject swordPrefab;

    public override void Attack()
    {
        if (!IsOffCooldown()) return;

        Vector3 origin = swordPrefab.transform.position;
        Vector3 forward = swordPrefab.transform.forward;

        Collider[] hits = Physics.OverlapSphere(origin + forward * (stats.reach * 0.5f), stats.reach, hitMask);
        foreach (var c in hits)
        {
            Vector3 toTarget = (c.transform.position - origin).normalized;
            if (Vector3.Angle(forward, toTarget) <= stats.arcDegrees * 0.5f)
            {
                var dmg = c.GetComponent<IDamageable>();
                if (dmg != null)
                    dmg.ApplyDamage(new DamagePayload { rawDamage = GetDamage(), instigator = swordPrefab });
            }
        }

        ResetCooldown();
    }
}