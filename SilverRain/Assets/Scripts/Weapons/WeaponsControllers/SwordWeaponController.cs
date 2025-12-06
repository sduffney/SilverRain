using System.Collections;
using UnityEngine;
public class SwordWeaponController : WeaponController
{
    [SerializeField] private Transform player;
    [SerializeField] private Collider col;
    [SerializeField] private Renderer ren;

    private float angle;
    private bool isAttacking = false;

    private void Start()
    {
        //Ensure components arent null
        if (player == null) player = GameManager.Instance.Player.transform;
        if (col == null) col = gameObject.GetComponent<Collider>();
        if (ren == null) ren = gameObject.GetComponent<Renderer>();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isAttacking || player == null || weaponData == null)
            return;

        angle += 180f * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;

        float radius = GetSize();    // size includes PlayerStats.size
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

        if (weaponData.CurrentLevel <= 0)
            return;

        gameObject.SetActive(true);
        StartCoroutine(OnDuration());
    }

    public override IEnumerator OnDuration()
    {
        Attack(); // start orbit
        float duration = GetDuration();
        if (duration > 0f)
            yield return new WaitForSeconds(duration);

        StartCoroutine(OnCoolDown());
    }

    public override IEnumerator OnCoolDown()
    {
        isAttacking = false;

        //Disable collider and renderer
        if (col != null) col.enabled = false;
        if (ren != null) ren.enabled = false;

        float cd = GetCooldown();
        if (cd > 0f)
            yield return new WaitForSeconds(cd);

        if (weaponData != null && weaponData.CurrentLevel > 0)
        {
            StartCoroutine(OnDuration());
        }
    }

    public override void Attack()
    {
        //Enable collider and renderer
        if (col != null) col.enabled = true;
        if (ren != null) ren.enabled = true;
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
            enemyHealth.TakeDamage(Mathf.RoundToInt(GetDamage()));
        }
    }
}