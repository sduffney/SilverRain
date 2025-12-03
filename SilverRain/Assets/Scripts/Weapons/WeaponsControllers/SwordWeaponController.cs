using UnityEngine;

public class SwordWeaponController : MonoBehaviour
{
    public SwordData weaponData;     // assign in code or Inspector
    public Transform player;         // auto-resolved if null

    private float activeUntil;
    private float angle;

    private void Awake()
    {
        if (player == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) player = go.transform;
        }
    }

    private void Update()
    {
        if (Time.time > activeUntil)
        {
            Destroy(gameObject);
            return;
        }

        // Orbit around player
        angle += 180f * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * weaponData.baseSize;
        transform.position = player.position + offset;

        // Look at player, then apply X = 90° offset
        Vector3 dir = (player.position - transform.position).normalized;
        Quaternion look = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = look * Quaternion.Euler(-90f, 0f, 0f);

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