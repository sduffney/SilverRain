using UnityEngine;

public class GunWeaponController : MonoBehaviour
{
    public TemporaryWeapon weaponData;   // configure baseDuration, baseDamage, projectileSpeed
    public Transform firePoint;          // optional; preferably a child at the muzzle
    public GameObject projectilePrefab;  // prefab with Projectile.cs

    private Transform player;
    private Transform cam;               // reference to the main camera
    private float activeUntil;

    // Tunables "in code"
    private float spawnOffsetForward = 1f; // forward distance from camera to place the weapon
    private float spawnOffsetUp = -0.4f;     // vertical offset relative to the camera
    private float muzzleOffset = 0.5f;       // if firePoint is missing, projectile spawns this far forward

    private void Awake()
    {
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go != null) player = go.transform;

        if (Camera.main != null) cam = Camera.main.transform;
        else Debug.LogWarning("GunWeaponController: no Camera.main found. Ensure a camera has the MainCamera tag.");

        if (firePoint == null) firePoint = transform; // fallback if no child firePoint assigned
    }

    private void Update()
    {
        if (Time.time > activeUntil)
        {
            Destroy(gameObject);
            return;
        }

        // Update weapon position and rotation to follow the camera (without parenting)
        if (cam != null)
        {
            Vector3 desiredPos = cam.position + cam.forward * spawnOffsetForward + cam.up * spawnOffsetUp;
            transform.position = desiredPos;
            transform.rotation = Quaternion.LookRotation(-cam.forward, Vector3.up);

            if (firePoint != null && firePoint != transform)
            {
                Vector3 muzzleWorldPos = transform.position + transform.forward * muzzleOffset;
                firePoint.position = muzzleWorldPos;
                firePoint.rotation = transform.rotation;
            }
        }
        else if (player != null)
        {
            // Fallback: follow player rotation if no camera is available
            transform.position = player.position + player.forward * spawnOffsetForward;
            transform.rotation = player.rotation;
        }
    }

    public void Activate()
    {
        if (weaponData == null) { Debug.LogError("weaponData not assigned"); return; }
        if (!weaponData.IsOffCooldown()) return;

        activeUntil = Time.time + weaponData.baseDuration;
        weaponData.ResetCooldown();

        // Force immediate update so firePoint world pos is correct before firing
        if (cam != null)
        {
            Vector3 desiredPos = cam.position + cam.forward * spawnOffsetForward + cam.up * spawnOffsetUp;
            transform.position = desiredPos;
            transform.rotation = Quaternion.LookRotation(cam.forward, Vector3.up);

            if (firePoint != null && firePoint != transform)
            {
                firePoint.position = transform.position + transform.forward * muzzleOffset;
                firePoint.rotation = transform.rotation;
            }
        }

        FireOnce();
    }

    private void FireOnce()
    {
        if (projectilePrefab == null) { Debug.LogError("Projectile prefab not assigned"); return; }

        Vector3 spawnPos;
        Quaternion spawnRot;
        Vector3 dir;

        if (firePoint != null)
        {
            spawnPos = firePoint.position;
            spawnRot = firePoint.rotation;
            dir = firePoint.forward;
        }
        else
        {
            spawnPos = transform.position + transform.forward * muzzleOffset;
            spawnRot = Quaternion.LookRotation(transform.forward);
            dir = transform.forward;
        }

        var projObj = Instantiate(projectilePrefab, spawnPos, spawnRot);
        var proj = projObj.GetComponent<Projectile>();
        if (proj == null) { Debug.LogError("Projectile.cs not found on instantiated prefab"); Destroy(projObj); return; }

        proj.Init(weaponData.GetDamage(), dir, weaponData.projectileSpeed);
    }
}
