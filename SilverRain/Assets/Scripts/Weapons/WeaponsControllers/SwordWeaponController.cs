using System.Collections;
using UnityEngine;
public class SwordWeaponController : WeaponController
{
    public TemporaryWeapon weaponData;  // SwordData should inherit from TemporaryWeapon
    public Transform player;

    private float angle;
    private bool isRunning = false;
    private bool isAttacking = false;

    private void Awake()
    {
        if (player == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) player = go.transform;
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isAttacking || player == null || weaponData == null)
            return;

        angle += 180f * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;

        float radius = weaponData.GetSize();    // size includes PlayerStats.size
        Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * radius;

        transform.position = player.position + offset;

        Vector3 dir = (player.position - transform.position).normalized;
        Quaternion look = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = look * Quaternion.Euler(-90f, 0f, 0f);
    }

    public override void OnActivate()
    {
        if (weaponData == null)
        {
            Debug.LogError("SwordWeaponController: weaponData not assigned.");
            return;
        }

        if (weaponData.GetCurrentLevel() <= 0)
            return;

        if (isRunning)
            return;

        gameObject.SetActive(true);
        isRunning = true;
        StartCoroutine(OnDuration());
    }

    public override IEnumerator OnDuration()
    {
        Attack(); // start orbit
        float duration = weaponData.GetDuration();
        if (duration > 0f)
            yield return new WaitForSeconds(duration);

        StartCoroutine(OnCoolDown());
    }

    public override IEnumerator OnCoolDown()
    {
        isAttacking = false;
        gameObject.SetActive(false);

        float cd = weaponData.GetCooldown();
        if (cd > 0f)
            yield return new WaitForSeconds(cd);

        if (weaponData != null && weaponData.GetCurrentLevel() > 0)
        {
            gameObject.SetActive(true);
            StartCoroutine(OnDuration());
        }
        else
        {
            isRunning = false;
        }
    }

    public override void Attack()
    {
        isAttacking = true;
        angle = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttacking || weaponData == null) return;

        var enemyHealth = other.GetComponent<EnemyHealth>() ?? other.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null)
        {
            // GetDamage() already uses PlayerStats.attackDamage
            enemyHealth.TakeDamage(Mathf.RoundToInt(weaponData.GetDamage()));
        }
    }
}
