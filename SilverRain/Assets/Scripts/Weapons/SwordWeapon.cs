using UnityEngine;

public class SwordWeapon : TempWeapon
{
    public LayerMask hitMask;

    public override void Attack()
    {
        if (!IsOffCooldown()) return;

        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;

        Collider[] hits = Physics.OverlapSphere(origin + forward * (stats.reach * 0.5f), stats.reach, hitMask);
        foreach (var c in hits)
        {
            Vector3 toTarget = (c.transform.position - origin).normalized;
            if (Vector3.Angle(forward, toTarget) <= stats.arcDegrees * 0.5f)
            {
                var dmg = c.GetComponent<IDamageable>();
                if (dmg != null)
                    dmg.ApplyDamage(new DamagePayload { rawDamage = GetDamage(), instigator = gameObject });
            }
        }

        ResetCooldown();
    }
}