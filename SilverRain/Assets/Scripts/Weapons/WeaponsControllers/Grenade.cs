using UnityEngine;

public class Grenade : MonoBehaviour
{
    private float damage;
    private float fuseTime;
    private bool hasExploded = false;

    public void Init(float dmg, float fuse)
    {
        damage = dmg;
        fuseTime = fuse;
        Invoke(nameof(Explode), fuseTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Optional: add bounce, stick, or early explosion logic here
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        Debug.Log("💥 Grenade exploded with damage: " + damage);

        // TODO: Add explosion radius, damage to nearby enemies, particles, sound, etc.

        Destroy(gameObject);
    }
}
