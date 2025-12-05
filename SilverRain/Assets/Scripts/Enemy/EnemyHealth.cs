using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private ParticleSystem bloodSplatterPrefab;

    [Header("Components")]
    public Animator animator;
    private Enemy enemy;
    private EnemyController controller;
    private PlayerLevel player;

    void Start()
    {
        currentHealth = maxHealth;
        enemy = GetComponent<Enemy>();
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<EnemyController>();
        player = FindFirstObjectByType<PlayerLevel>();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            //Debug.Log("We should die now");
            Die();
        }

        //Instatntiate BloodSplatter
        Vector3 bloodSplatterSpawn = transform.position;
        bloodSplatterSpawn.y += 2;
        Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
        var bloodSplatter = Instantiate(bloodSplatterPrefab, bloodSplatterSpawn, rotation);

        bloodSplatter.Play();

        //Reveal this enemy
        if (!GlobalInvisibilityManager.Instance.isActive)
        {
            enemy.RevealTimed(5f);
        }


        //Play hurt animation
        animator.SetTrigger("hurt");
    }
    private void Die()
    {
        Collider collider = GetComponent<Collider>();
        Destroy(collider);
        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        //Debug.Log("We are in the death Corutine");
        animator.SetBool("isDead", true);
        NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
        Destroy(agent);
        Destroy(controller);
        player.GainXP(enemy.RewardXP());
        GameManager.Instance.AddScore(enemy.RewardScore());
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    public void DamageTest() 
    {
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            TakeDamage(1);
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(currentHealth);
        }
    }

}
