using System.Collections;
using UnityEngine;

public class GrenadeWeaponController : WeaponController
{
    [Header("Data & Prefab")]
    public GrenadeData grenadeData;   // ScriptableObject (should behave like TemporaryWeapon)
    public GameObject grenadePrefab;  // Prefab with Rigidbody + Grenade.cs

    [Header("Throw Offsets (from camera)")]
    public float forwardOffset = 1.2f;
    public float upwardOffset = 0.0f;

    private Transform cam;
    private bool isRunning = false;

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
        gameObject.SetActive(false);
    }

    public override void OnActivate()
    {
        if (grenadeData == null)
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
        yield return new WaitForSeconds(grenadeData.GetDuration());
        StartCoroutine(OnCoolDown());
    }

    public override IEnumerator OnCoolDown()
    {
        yield return new WaitForSeconds(grenadeData.GetCooldown());
        StartCoroutine(OnDuration());
    }

    public override void Attack()
    {
        Vector3 spawnPos = cam.position + cam.forward * forwardOffset + cam.up * upwardOffset;
        Quaternion spawnRot = cam.rotation;

        GameObject grenade = Instantiate(grenadePrefab, spawnPos, spawnRot);

        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("GrenadeWeaponController: grenadePrefab has no Rigidbody.");
            return;
        }

        // Build throw direction: tuned in GrenadeData
        Vector3 throwDirection =
            (cam.forward * grenadeData.throwForce) +
            (cam.up * grenadeData.upwardForce);

        rb.linearVelocity = throwDirection * 0.8f;
        rb.angularVelocity = Random.insideUnitSphere * 5f;

        // Initialize grenade behaviour
        Grenade grenadeScript = grenade.GetComponent<Grenade>();
        if (grenadeScript != null)
        {
            grenadeScript.Init(grenadeData.GetDamage(), grenadeData.baseDuration);
        }
    }
}
