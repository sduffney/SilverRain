using UnityEngine;

//public class SwordKeySpawner : MonoBehaviour
//{
//    public SwordData swordData;       // assign ScriptableObject asset
//    public GameObject swordPrefab;    // assign the sword prefab
//    public KeyCode attackKey = KeyCode.Q;

//    private void Update()
//    {
//        if (Input.GetKeyDown(attackKey))
//        {
//            if (!swordData.IsOffCooldown()) return;

//            var sword = Instantiate(swordPrefab, transform.position, Quaternion.identity);
//            var controller = sword.GetComponent<SwordWeaponController>();
//            controller.weaponData = swordData;
//            controller.Activate(); // player is auto-found via tag
//        }
//    }
//}
//public class GunKeySpawner : MonoBehaviour
//{
//    public TemporaryWeapon gunData;   // ScriptableObject configured for gun
//    public GameObject gunPrefab;      // Prefab with GunWeaponController
//    public KeyCode attackKey = KeyCode.E;

//    private void Update()
//    {
//        if (Input.GetKeyDown(attackKey))
//        {
//            if (!gunData.IsOffCooldown()) return;
//            var gun = Instantiate(gunPrefab, transform.position, Quaternion.identity);
//            var controller = gun.GetComponent<GunWeaponController>();
//            controller.weaponData = gunData;
//            controller.Activate();  // controller will auto-find player via tag
//        }
//    }
//}

using UnityEngine;

public class GrenadeKeySpawner : MonoBehaviour
{
    public TemporaryWeapon grenadeData;   // ScriptableObject configured for grenade
    public GameObject grenadeControllerPrefab; // Prefab with GrenadeWeaponController attached
    public KeyCode attackKey = KeyCode.R;

    private Transform cam;

    private void Awake()
    {
        if (Camera.main != null) cam = Camera.main.transform;
        else Debug.LogWarning("GrenadeKeySpawner: no Camera.main found.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            if (cam == null || grenadeControllerPrefab == null || grenadeData == null) return;
            if (!grenadeData.IsOffCooldown()) return;

            // Spawn the controller in front of the camera
            Vector3 spawnPos = cam.position + cam.forward * 1.2f;
            var grenadeController = Instantiate(grenadeControllerPrefab, spawnPos, Quaternion.identity)
                                     .GetComponent<GrenadeWeaponController>();

            grenadeController.weaponData = grenadeData;
            grenadeController.Activate();
        }
    }
}
