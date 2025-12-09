using System.Collections;
using UnityEngine;

public class GrenadeWeaponController : WeaponController
{
    [Header("Data & Prefab")]
    public GameObject grenadePrefab;
    [SerializeField] private AudioClip attackSound;

    [Header("Throw Settings")]
    public float throwForce = 12f;
    public float upwardForce = 4f;
    public float forwardOffset = 1.2f;
    public float upwardOffset = 0.0f;

    [SerializeField] private Transform cam;
    private AudioSource audioSource;

    private void Start()
    {
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
        else
        {
            Debug.LogError("GrenadeWeaponController: No MainCamera found. Tag your camera as MainCamera.");
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        gameObject.SetActive(false);
    }

    public override void OnActivate()
    {
        if (weaponData == null)
        {
            Debug.LogError("GrenadeWeaponController: grenadeData is not assigned.");
            return;
        }
        gameObject.SetActive(true);
        StartCoroutine(OnDuration());
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
        Vector3 spawnPos = cam.position + cam.forward * forwardOffset + cam.up * upwardOffset;
        Quaternion spawnRot = cam.rotation;

        GameObject grenade = Instantiate(grenadePrefab, spawnPos, spawnRot);

        if (attackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("GrenadeWeaponController: grenadePrefab has no Rigidbody.");
            return;
        }

        Vector3 throwDirection =
            (cam.forward * throwForce) +
            (cam.up * upwardForce);

        rb.linearVelocity = throwDirection * 0.8f;
        rb.angularVelocity = Random.insideUnitSphere * 5f;

        Grenade grenadeScript = grenade.GetComponent<Grenade>();
        if (grenadeScript != null)
        {
            grenadeScript.Init(GetDamage(), weaponData.BaseDuration);
        }
    }
}