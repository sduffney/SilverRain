using UnityEngine;

public class SwordWeaponController : MonoBehaviour
{
    public TemporaryWeapon weaponData;   // ScriptableObject with stats
    public Transform player;             // assign Player transform in Inspector

    private float activeUntil;
    private float angle;

    private void Update()
    {
        // If sword expired, destroy it
        if (weaponData.baseDuration > 0 && Time.time > activeUntil)
        {
            Destroy(gameObject);
            return;
        }

        // Orbit logic
        angle += 180f * Time.deltaTime; // orbit speed
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * weaponData.baseSize;
        transform.position = player.position + offset;
        transform.LookAt(player.position);
    }

    public void Activate()
    {
        if (!weaponData.IsOffCooldown()) return;

        activeUntil = Time.time + weaponData.baseDuration;
        weaponData.ResetCooldown();
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(Mathf.RoundToInt(weaponData.GetDamage()));
        }
    }
}