using UnityEngine;

public class SwordKeySpawner : MonoBehaviour
{
    public SwordData swordData;       // assign ScriptableObject asset
    public GameObject swordPrefab;    // assign the sword prefab
    public KeyCode attackKey = KeyCode.Q;

    private void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            if (!swordData.IsOffCooldown()) return;

            var sword = Instantiate(swordPrefab, transform.position, Quaternion.identity);
            var controller = sword.GetComponent<SwordWeaponController>();
            controller.weaponData = swordData;
            controller.Activate(); // player is auto-found via tag
        }
    }
}