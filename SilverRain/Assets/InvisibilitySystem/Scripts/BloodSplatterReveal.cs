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
    //private void OnParticleTrigger()
    //{
    //    Debug.Log("Trigger");
    //    int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);

    //    for (int i = 0; i < numEnter; i++)
    //    {
    //        ParticleSystem.Particle p = particles[i];

    //        Collider[] colliders = Physics.OverlapSphere(p.position, 1.0f);
    //        foreach (var col in colliders)
    //        {
    //            Enemy enemy = col.GetComponent<Enemy>();
    //            if (enemy != null)
    //            {
    //                enemy.Reveal();
    //            }
    //        }
    //    }
    //}
}
