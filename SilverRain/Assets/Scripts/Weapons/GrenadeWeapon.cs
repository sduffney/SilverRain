using UnityEngine;

public class GrenadeWeapon : TempWeapon
{
    public Transform firePoint;
    public GameObject grenadePrefab;

    public override void Attack()
    {
        if (!IsOffCooldown()) return;

        var grenadeObj = Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);
        var proj = grenadeObj.GetComponent<Projectile>();
        proj.Init(GetDamage(), firePoint.forward, stats.projectileSpeed, isExplosive: true, aoeRadius: stats.aoeRadius);

        ResetCooldown();
    }
}