using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    public Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        //Activate reveal particles
        //Reveal enemy
        //Play hurt animation
    }
    private void Die()
    {
        Destroy(gameObject);
        //Play death animation
    }

}
