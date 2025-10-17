using UnityEngine;

public class BloodSplatterReveal : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null) return; 
        else enemy.Reveal();

    }
}
