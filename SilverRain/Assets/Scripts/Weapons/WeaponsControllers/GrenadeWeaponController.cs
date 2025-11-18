using UnityEngine;

public class GrenadeWeaponController : MonoBehaviour
{
    public TemporaryWeapon weaponData;
    public GameObject grenadePrefab;

    private Transform cam;
    private float activeUntil;

    // Throw tuning
    private float forwardOffset = 1.2f;
    private float throwForce = 12f;
    private float upwardForce = 4f;

    private void Awake()
    {
        if (Camera.main != null) cam = Camera.main.transform;
        else Debug.LogWarning("GrenadeWeaponController: no Camera.main found.");
    }

    private void Update()
    {
        if (Time.time > activeUntil)
        {
            Destroy(gameObject);
        }
    }

    public void Activate()
    {

        //add player states
        if (weaponData == null || !weaponData.IsOffCooldown()) return;

        weaponData.ResetCooldown();
        activeUntil = Time.time + weaponData.baseDuration;

        ThrowGrenade();
    }

    private void ThrowGrenade()
    {
        if (grenadePrefab == null || cam == null || weaponData == null) return;

        // Casteamos el ScriptableObject para acceder a los valores espec�ficos de la granada
        GrenadeData grenadeStats = weaponData as GrenadeData;
        if (grenadeStats == null) { Debug.LogError("weaponData is not GrenadeData"); return; }

        Vector3 spawnPos = cam.position + cam.forward * 1.2f;
        GameObject grenade = Instantiate(grenadePrefab, spawnPos, Quaternion.identity);

        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        if (rb == null) { Debug.LogError("Grenade prefab missing Rigidbody"); return; }

        // Usamos los valores del ScriptableObject para calcular la direcci�n del lanzamiento
        Vector3 throwDirection = cam.forward * grenadeStats.throwForce + cam.up * grenadeStats.upwardForce;
        rb.linearVelocity = throwDirection;
        rb.AddForce(throwDirection, ForceMode.VelocityChange);

        Grenade grenadeScript = grenade.GetComponent<Grenade>();
        if (grenadeScript != null)
        {
            grenadeScript.Init(grenadeStats.GetDamage(), grenadeStats.baseDuration);
        }
    }

}
