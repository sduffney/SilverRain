using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    [Header("Sword")]
    public SwordData swordData;
    public GameObject swordPrefab;
    public KeyCode swordKey = KeyCode.Q;

    [Header("Gun")]
    public TemporaryWeapon gunData;   // GunData
    public GameObject gunPrefab;
    public KeyCode gunKey = KeyCode.E;

    [Header("Grenade")]
    public GrenadeData grenadeData;              // ScriptableObject for grenade
    public GameObject grenadeControllerPrefab;   // (no longer used to spawn, but can leave in inspector)
    public KeyCode grenadeKey = KeyCode.R;

    private Transform cam;
    private GrenadeWeaponController grenadeController;

    private void Awake()
    {
        // Camera for direction (grenade controller probably also uses this)
        if (Camera.main != null)
            cam = Camera.main.transform;
        else
            Debug.LogWarning("KeySpawner: no Camera.main found (grenade may use camera).");

        // Find the existing GrenadeWeaponController in the scene
        grenadeController = FindAnyObjectByType<GrenadeWeaponController>();
        if (grenadeController == null)
        {
            Debug.LogWarning("KeySpawner: No GrenadeWeaponController found in scene. Grenade key will do nothing.");
        }
    }

    void Update()
    {
        HandleSword();
        HandleGun();
        HandleGrenade();
    }

    // ---------- SWORD ----------
    private void HandleSword()
    {
        if (!Input.GetKeyDown(swordKey)) return;
        if (swordData == null || swordPrefab == null)
        {
            Debug.LogError("KeySpawner: Missing swordData or swordPrefab.");
            return;
        }
        if (!swordData.IsOffCooldown()) return;

        GameObject sword = Instantiate(swordPrefab);

        var controller = sword.GetComponent<SwordWeaponController>();
        if (controller == null)
        {
            Debug.LogError("KeySpawner: Sword prefab missing SwordWeaponController.");
            return;
        }

        controller.weaponData = swordData;
        controller.Activate();
    }

    // ---------- GUN ----------
    private void HandleGun()
    {
        if (!Input.GetKeyDown(gunKey)) return;
        if (gunData == null || gunPrefab == null)
        {
            Debug.LogError("KeySpawner: Missing gunData or gunPrefab.");
            return;
        }
        if (!gunData.IsOffCooldown()) return;

        GameObject gun = Instantiate(gunPrefab);

        var controller = gun.GetComponent<GunWeaponController>();
        if (controller == null)
        {
            Debug.LogError("KeySpawner: Gun prefab missing GunWeaponController.");
            return;
        }

        controller.weaponData = gunData;
        controller.Activate();
    }

    // ---------- GRENADE (only one, with correct physics) ----------
    private void HandleGrenade()
    {
        if (!Input.GetKeyDown(grenadeKey)) return;

        if (grenadeData == null)
        {
            Debug.LogError("KeySpawner: grenadeData is not assigned.");
            return;
        }

        if (grenadeController == null)
        {
            Debug.LogError("KeySpawner: No GrenadeWeaponController found in scene.");
            return;
        }

        if (!grenadeData.IsOffCooldown()) return;

        // Just tell the existing controller to throw the grenade
        grenadeController.grenadeData = grenadeData;
        grenadeController.Activate();
    }
}




//public class SwordKeySpawner : MonoBehaviour
//{
//    public SwordData swordData;       // ScriptableObject asset
//    public GameObject swordPrefab;    // Prefab with SwordWeaponController
//    public KeyCode attackKey = KeyCode.Q;

//    private void Update()
//    {
//        if (!Input.GetKeyDown(attackKey))
//            return;

//        if (swordData == null || swordPrefab == null)
//        {
//            Debug.LogError("SwordKeySpawner: Missing swordData or swordPrefab.");
//            return;
//        }

//        if (!swordData.IsOffCooldown())
//            return;

//        // Just spawn the prefab – controller will handle position/rotation
//        GameObject sword = Instantiate(swordPrefab);

//        SwordWeaponController controller = sword.GetComponent<SwordWeaponController>();
//        if (controller == null)
//        {
//            Debug.LogError("SwordKeySpawner: swordPrefab has no SwordWeaponController.");
//            return;
//        }

//        controller.weaponData = swordData;
//        controller.Activate();
//    }
//}

//public class GunKeySpawner : MonoBehaviour
//{
//    public TemporaryWeapon gunData;   // ScriptableObject (GunData)
//    public GameObject gunPrefab;      // Prefab with GunWeaponController
//    public KeyCode attackKey = KeyCode.E;

//    private void Update()
//    {
//        if (!Input.GetKeyDown(attackKey))
//            return;

//        if (gunData == null || gunPrefab == null)
//        {
//            Debug.LogError("GunKeySpawner: Missing gunData or gunPrefab.");
//            return;
//        }

//        if (!gunData.IsOffCooldown())
//            return;

//        // Just spawn the prefab – controller will handle position/rotation/projectiles
//        GameObject gun = Instantiate(gunPrefab);

//        GunWeaponController controller = gun.GetComponent<GunWeaponController>();
//        if (controller == null)
//        {
//            Debug.LogError("GunKeySpawner: gunPrefab has no GunWeaponController.");
//            return;
//        }

//        controller.weaponData = gunData;
//        controller.Activate();
//    }
//}

//public class GrenadeKeySpawner : MonoBehaviour
//{
//    public GrenadeData grenadeData;              // ScriptableObject for grenade
//    public GameObject grenadeControllerPrefab;   // Prefab with GrenadeWeaponController
//    public KeyCode attackKey = KeyCode.R;

//    private void Update()
//    {
//        if (!Input.GetKeyDown(attackKey))
//            return;

//        if (grenadeData == null)
//        {
//            Debug.LogError("GrenadeKeySpawner: grenadeData is not assigned.");
//            return;
//        }

//        if (grenadeControllerPrefab == null)
//        {
//            Debug.LogError("GrenadeKeySpawner: grenadeControllerPrefab is not assigned.");
//            return;
//        }

//        // Spawn the controller (it will throw once Activate() is called)
//        GameObject controllerObj = Instantiate(
//            grenadeControllerPrefab,
//            transform.position,
//            Quaternion.identity
//        );

//        GrenadeWeaponController controller = controllerObj.GetComponent<GrenadeWeaponController>();
//        if (controller == null)
//        {
//            Debug.LogError("GrenadeKeySpawner: grenadeControllerPrefab is missing GrenadeWeaponController.");
//            Destroy(controllerObj);
//            return;
//        }

//        controller.grenadeData = grenadeData;
//        controller.Activate();

//        // Optional: auto-cleanup after a few seconds
//        Destroy(controllerObj, 3f);
//    }
//}