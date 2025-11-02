using UnityEngine;

public class GunWeapon : TemporaryWeapon
{
    public Transform firePoint;
    public GameObject projectilePrefab;

    public override void Attack()
    {
        if (!IsOffCooldown()) return;

        var projObj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        var proj = projObj.GetComponent<Projectile>();
        proj.Init(GetDamage(), firePoint.forward, projectileSpeed);

        ResetCooldown();
    }
}