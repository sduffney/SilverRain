using System.Collections;
using UnityEngine;

public class GunWeaponController : WeaponController
{
    public Transform firePoint;          // optional; preferably a child at the muzzle
    public GameObject projectilePrefab;  // prefab with Projectile.cs

    private Transform playerTrans;
    private Transform cam;

    // Tunables "in code"
    private float spawnOffsetForward = 1f; // forward distance from camera to place the weapon
    private float spawnOffsetUp = -0.4f;     // vertical offset relative to the camera

    public GameObject player;
    private void Start()
    {
        player = GameManager.Instance.Player;
        if (player != null) {
            playerTrans = player.transform;
            //print("Found player transform for GunWeaponController.");
        }

        if (Camera.main != null) cam = Camera.main.transform;
        else Debug.LogWarning("GunWeaponController: no Camera.main found. Ensure a camera has the MainCamera tag.");

        if (firePoint == null) firePoint = transform; // fallback if no child firePoint assigned
    }

    private void Update()
    {

        // Update weapon position and rotation to follow the camera (without parenting)
        if (cam != null)
        {
            Vector3 desiredPos = cam.position + cam.forward * spawnOffsetForward + cam.up * spawnOffsetUp;
            transform.position = desiredPos;
            transform.rotation = Quaternion.LookRotation(-cam.forward, Vector3.up);

        }
        else if (playerTrans != null)
        {
            // Fallback: follow playerTrans rotation if no camera is available
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

        var proj = projObj.GetComponent<Projectile>();
        if (proj == null)
        {
            Destroy(projObj);
            return;
        }

        float projectileSpeedBonus = player.GetComponent<PlayerStats>().projectileSpeed;
        proj.Init(GetDamage(), firePoint.forward , weaponData.BaseProjectileSpeed + projectileSpeedBonus);
    }
}