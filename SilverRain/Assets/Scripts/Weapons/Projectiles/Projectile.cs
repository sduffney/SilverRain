using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;
    private Vector3 direction;
    private float speed;

    [SerializeField] private float lifeTime = 5f;

    public void Init(float dmg, Vector3 dir, float spd)
    {
        damage = dmg;
        direction = dir.normalized;
        speed = spd;
    }

    private void Start()
    {
        if (lifeTime > 0f)
            Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    // If the projectile uses a non-trigger collider
    private void OnCollisionEnter(Collision col)
    {
        HandleHit(col.collider);
    }

    // If the projectile uses a trigger collider
    private void OnTriggerEnter(Collider other)
    {
        HandleHit(other);
    }

    private void HandleHit(Collider hit)
    {
        // Try to find EnemyHealth on the object or its parent
        var enemyHealth = hit.GetComponent<EnemyHealth>() ?? hit.GetComponentInParent<EnemyHealth>();

        if (enemyHealth != null)
        {
            // Same idea as the sword: apply damage, enemy handles animations
            enemyHealth.TakeDamage(Mathf.RoundToInt(damage));
        }

        // Bullet always disappears on first impact (enemy, wall, whatever)
        Destroy(gameObject);
    }
}
