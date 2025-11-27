using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] 
    private ParticleSystem bloodSplatterPrefab;
    public Animator animator;
    private Enemy enemy;

    void Start()
    {
        currentHealth = maxHealth;
        enemy = GetComponent<Enemy>();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }

        //Instatntiate BloodSplatter
        Vector3 bloodSplatterSpawn = transform.position;
        bloodSplatterSpawn.y += 2;
        var bloodSplatter = Instantiate(bloodSplatterPrefab, bloodSplatterSpawn, Quaternion.identity);

        bloodSplatter.Play();

        //Reveal this enemy
        if (!GlobalInvisibilityManager.Instance.isActive)
        {
            enemy.RevealTimed(5f);
        }


        //Play hurt animation
    }
    private void Die()
    {
        Destroy(gameObject);
        //Play death animation
    }

}
