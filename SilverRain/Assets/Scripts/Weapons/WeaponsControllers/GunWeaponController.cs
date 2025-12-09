using System.Collections;
using UnityEngine;

public class GunWeaponController : WeaponController
{
    public Transform firePoint;
    public GameObject projectilePrefab;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip attackSound;
    [Range(0f, 1f)][SerializeField] private float soundVolume = 0.5f; // Slider to control volume

    private Transform playerTrans;
    private Transform cam;
    private AudioSource audioSource;

    private float spawnOffsetForward = 1f;
    private float spawnOffsetUp = -0.4f;

    public GameObject player;

    private void Start()
    {
        player = GameManager.Instance.Player;
        if (player != null)
        {
            playerTrans = player.transform;
        }

        if (Camera.main != null) cam = Camera.main.transform;
        else Debug.LogWarning("GunWeaponController: no Camera.main found. Ensure a camera has the MainCamera tag.");

        if (firePoint == null) firePoint = transform;

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (cam != null)
        {
            Vector3 desiredPos = cam.position + cam.forward * spawnOffsetForward + cam.up * spawnOffsetUp;
            transform.position = desiredPos;
            transform.rotation = Quaternion.LookRotation(-cam.forward, Vector3.up);

        }
        else if (playerTrans != null)
        {
            transform.position = playerTrans.position + playerTrans.forward * spawnOffsetForward;
            transform.rotation = playerTrans.rotation;
        }
    }

    public override void OnActivate()
    {
        if (weaponData == null) { Debug.LogError("weaponData not assigned"); return; }
        gameObject.SetActive(true);
        StartCoroutine(OnCoolDown());
    }

    public override IEnumerator OnDuration()
    {
        Attack();
        yield return new WaitForSeconds(GetDuration());
        StartCoroutine(OnCoolDown());
    }

    public override IEnumerator OnCoolDown()
    {
        yield return new WaitForSeconds(GetCooldown());
        StartCoroutine(OnDuration());
    }

    public override void Attack()
    {
        Quaternion spawnRot = Quaternion.LookRotation(firePoint.forward, Vector3.up) * Quaternion.Euler(90f, 0f, 0f);
        var projObj = Instantiate(projectilePrefab, firePoint.position, spawnRot);

        // Play sound with the specific volume
        if (attackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSound, soundVolume);
        }

        var proj = projObj.GetComponent<Projectile>();
        if (proj == null)
        {
            Destroy(projObj);
            return;
        }

        float projectileSpeedBonus = player.GetComponent<PlayerStats>().projectileSpeed;
        proj.Init(GetDamage(), firePoint.forward, weaponData.BaseProjectileSpeed + projectileSpeedBonus);
    }
}