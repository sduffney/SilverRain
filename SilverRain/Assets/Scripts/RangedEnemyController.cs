using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyController : EnemyController
{
    private float shootTimer = 0f;
    [SerializeField]
    private float timeBetweenShots = 2f;
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    GameObject projectilePrefab;

    public override void Attack(PlayerHealth player)
    {
        shootTimer += Time.deltaTime;
        //Spawn projectile targetting player.
        if (shootTimer >= timeBetweenShots) 
        {
            if(targetPlayer != null) 
            {
                animator.SetTrigger("attacking");
                Vector3 dir = (targetPlayer.transform.position - firePoint.position).normalized;
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                projectile.GetComponent<EnemyProjectile>().Initialize(dir, enemy.damage, player);
            }
            shootTimer = 0;
        }

    }

    public override void Move()
    {
        //Move towards the player
        agent.SetDestination(targetPlayer.transform.position);
        float speed = 0f;
        speed = agent.velocity.magnitude;
        animator.SetFloat("speed", speed);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerHealth>()) 
        {
            PlayerHealth target = other.GetComponent<PlayerHealth>();
            Attack(target);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        shootTimer = 0f;
    }
}
