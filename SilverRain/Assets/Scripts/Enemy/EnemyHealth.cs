using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private ParticleSystem bloodSplatterPrefab;
    [SerializeField] private AudioClip hurtSound;

    [Header("Components")]
    public Animator animator;
    private Enemy enemy;
    private EnemyController controller;
    private PlayerLevel player;
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        enemy = GetComponent<Enemy>();
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<EnemyController>();
        player = FindFirstObjectByType<PlayerLevel>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (hurtSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        Vector3 bloodSplatterSpawn = transform.position;
        bloodSplatterSpawn.y += 1f;
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
        var bloodSplatter = Instantiate(bloodSplatterPrefab, bloodSplatterSpawn, rotation);

        bloodSplatter.Play();

        if (!GlobalInvisibilityManager.Instance.isActive)
        {
            enemy.RevealTimed(5f);
        }

        animator.SetTrigger("hurt");
    }
    private void Die()
    {
        foreach (var col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        var agent = GetComponent<NavMeshAgent>();
        if (agent != null) Destroy(agent);
        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        animator.SetBool("isDead", true);
        Destroy(controller);
        player.GainXP(enemy.RewardXP());
        GameManager.Instance.AddScore(enemy.RewardScore());
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}