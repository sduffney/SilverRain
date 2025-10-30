using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Reveal();
        }
    }
}
