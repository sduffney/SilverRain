using System.Collections;
using UnityEngine;

public class GunWeaponController : WeaponController
{
    public TemporaryWeapon weaponData;   // configure baseDuration, baseDamage, projectileSpeed
    public Transform firePoint;          // optional; preferably a child at the muzzle
    public GameObject projectilePrefab;  // prefab with Projectile.cs

    private Transform playerTrans;
    private Transform cam;               // reference to the main camera
    private float activeUntil;

    // Tunables "in code"
    private float spawnOffsetForward = 1f; // forward distance from camera to place the weapon
    private float spawnOffsetUp = -0.4f;     // vertical offset relative to the camera
    private float muzzleOffset = 0.5f;       // if firePoint is missing, projectile spawns this far forward

    public GameObject player;
    private void Start()
    {
        print("Hello");
        player = GameManager.Instance.player;
        //if (player != null) {
            playerTrans = player.transform;
            print("Found player transform for GunWeaponController.");
        //}

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

            //if (firePoint != null && firePoint != transform)
            //{
            //    Vector3 muzzleWorldPos = transform.position + transform.forward * muzzleOffset;
            //    firePoint.position = muzzleWorldPos;
            //    firePoint.rotation = transform.rotation;
            //}
        }
        else if (playerTrans != null)
        {
            // Fallback: follow playerTrans rotation if no camera is available
            transform.position = playerTrans.position + playerTrans.forward * spawnOffsetForward;
            transform.rotation = playerTrans.rotation;
        }
    }

    //public void Activate()
    //{
    //    if (weaponData == null) { Debug.LogError("weaponData not assigned"); return; }
    //    if (!weaponData.IsOffCooldown()) return;

    //    activeUntil = Time.time + weaponData.baseDuration;
    //    weaponData.ResetCooldown();
    //    if (cam != null)
    //    {
    //        Vector3 desiredPos = cam.position + cam.forward * spawnOffsetForward + cam.up * spawnOffsetUp;
    //        transform.position = desiredPos;
    //        transform.rotation = Quaternion.LookRotation(cam.forward, Vector3.up);

    //        if (firePoint != null && firePoint != transform)
    //        {
    //            firePoint.position = transform.position + transform.forward * muzzleOffset;
    //            firePoint.rotation = transform.rotation;
    //        }
    //    }

    //    FireOnce();
    //}

    private void FireOnce()
    {
        //if (projectilePrefab == null) return;

        //Vector3 spawnPos;
        //Quaternion spawnRot;
        //Vector3 dir;

        //if (firePoint != null)
        //{
        //    spawnPos = firePoint.position;
        //    dir = firePoint.forward;
        //}
        //else
        //{
        //    spawnPos = transform.position + transform.forward * muzzleOffset;
        //    dir = transform.forward;
        //}
        //spawnRot = Quaternion.LookRotation(dir, Vector3.up) * Quaternion.Euler(90f, 0f, 0f);

        //var projObj = Instantiate(projectilePrefab, spawnPos, spawnRot);

        //var proj = projObj.GetComponent<Projectile>();
        //if (proj == null)
        //{
        //    Destroy(projObj);
        //    return;
        //}

        //float projectileSpeedBonus = player.GetComponent<PlayerStats>().projectileSpeed;
        //proj.Init(weaponData.GetDamage(), dir, weaponData.projectileSpeed + projectileSpeedBonus);
    }

    public override void OnActivate()
    {
        if (weaponData == null) { Debug.LogError("weaponData not assigned"); return; }
        //GameObject gun = Instantiate(weaponData.weaponPrefab, Vector3.zero, Quaternion.identity, player.transform);
        gameObject.SetActive(true);
        StartCoroutine(OnCoolDown());
    }

    public override IEnumerator OnDuration()
    {
        Attack();
        yield return new WaitForSeconds(weaponData.GetDuration());
        StartCoroutine(OnCoolDown());
    }

    public override IEnumerator OnCoolDown()
    {
        yield return new WaitForSeconds(weaponData.GetCooldown());
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
        proj.Init(weaponData.GetDamage(), firePoint.forward , weaponData.projectileSpeed + projectileSpeedBonus);
    }
}