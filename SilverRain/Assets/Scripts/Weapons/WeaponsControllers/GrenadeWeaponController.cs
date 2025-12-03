using UnityEngine;

public class GrenadeWeaponController : MonoBehaviour
{
    [Header("Data & Prefab")]
    public GrenadeData grenadeData;   // ScriptableObject with damage, cooldown, throwForce, upwardForce
    public GameObject grenadePrefab;  // Prefab with Rigidbody + Grenade.cs

    [Header("Throw Offsets (from camera)")]
    public float forwardOffset = 1.2f;
    public float upwardOffset = 0.0f;

    private Transform cam;

    private void Awake()
    {
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
        else
        {
            Debug.LogError("GrenadeWeaponController: No MainCamera found. Tag your camera as MainCamera.");
        }
    }

    // Called by the KeySpawner when we want to throw
    public void Activate()
    {
        if (grenadeData == null)
        {
            Debug.LogError("GrenadeWeaponController: grenadeData is not assigned.");
            return;
        }

        if (!grenadeData.IsOffCooldown())
        {
            Debug.Log($"Grenade on cooldown ({grenadeData.GetCooldown():0.00}s).");
            return;
        }

        grenadeData.ResetCooldown();
        ThrowGrenade();
    }

    private void ThrowGrenade()
    {
        if (grenadePrefab == null)
        {
            Debug.LogError("GrenadeWeaponController: grenadePrefab is not assigned.");
            return;
        }

        if (cam == null)
        {
            Debug.LogError("GrenadeWeaponController: camera reference is missing.");
            return;
        }

        // Spawn position in front of the camera
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

        // If you liked the slower throw, keep a multiplier here:
        rb.linearVelocity = throwDirection * 0.8f;

        // Optional: random spin
        rb.angularVelocity = Random.insideUnitSphere * 5f;

        // Initialize grenade behaviour
        Grenade grenadeScript = grenade.GetComponent<Grenade>();
        if (grenadeScript != null)
        {
            grenadeScript.Init(grenadeData.GetDamage(), grenadeData.baseDuration);
        }
    }
}
