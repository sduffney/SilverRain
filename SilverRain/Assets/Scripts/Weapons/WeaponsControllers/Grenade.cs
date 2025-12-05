using UnityEngine;

public class Grenade : MonoBehaviour
{
    private float damage;
    private float lifeTime;

    [Header("Explosion Settings")]
    public float explosionRadius = 3f;
    public LayerMask hitMask; // Optional: set to Enemy layer in Inspector

    [Header("VFX")]
    public GameObject explosionVfxPrefab; // <-- assign in Inspector

    private bool hasExploded = false;

    public void Init(float dmg, float duration)
    {
        damage = dmg;
        lifeTime = duration;

        // Explode automatically after lifetime, in case it never hits anything
        Invoke(nameof(Explode), lifeTime);
    }

    // Non-trigger collider hit (e.g., ground, walls, enemies)
    private void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded)
        {
            Explode();
        }
    }

    // Trigger collider hit (if you mark the grenade collider as trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (!hasExploded)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // --- VFX: spawn explosion and scale it by explosionRadius ---
        if (explosionVfxPrefab != null)
        {
            GameObject vfx = Instantiate(explosionVfxPrefab, transform.position, Quaternion.identity);

            // Scale VFX so it roughly matches the explosion area
            vfx.transform.localScale = Vector3.one * explosionRadius;

            // If your prefab doesn't auto-destroy, you can uncomment this:
            // Destroy(vfx, 2f);
        }

        // --- Damage enemies in radius ---
        Collider[] hits;
        if (hitMask.value != 0)
        {
            hits = Physics.OverlapSphere(transform.position, explosionRadius, hitMask);
        }
        else
        {
            hits = Physics.OverlapSphere(transform.position, explosionRadius);
        }

        foreach (var h in hits)
        {
            var enemyHealth = h.GetComponent<EnemyHealth>() ?? h.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(Mathf.RoundToInt(damage));
            }
        }

        // Destroy the grenade object itself
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
