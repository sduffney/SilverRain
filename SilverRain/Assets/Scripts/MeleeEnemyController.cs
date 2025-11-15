using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyController : EnemyController
{
    private float meleeTimer = 0f;
    [SerializeField]
    private float timeBetweenAttacks = 1f;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerStay(UnityEngine.Collider collision)
    {
        
        Debug.Log("We have entered");
        //Check if player
        if (collision.gameObject.GetComponent<PlayerHealth>()) 
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            Debug.Log("Found Health");
            //Add timer to deal damage
            meleeTimer += Time.deltaTime;
            animator.SetBool("isAttacking", true);

            //Check if time has passed;
            if (meleeTimer >= timeBetweenAttacks)
            {
                Debug.Log("Timer passed");
                //Attack
                Attack(player);
                
            }

        }

    }

    private void OnTriggerExit(Collider collision)
    {
        animator.SetBool("isAttacking", false);
        meleeTimer = 0f;
    }

    private void Update()
    {
        Move();
    }
    public override void Attack(PlayerHealth player)
    {
        Debug.Log("Now Attacking");
        //Deal damage
        player.TakeDamage(enemy.damage);
        //Reset timer
        meleeTimer = 0;
    }

    public override void Move()
    {
        //Move towards the player
        agent.SetDestination(targetPlayer.transform.position);
        float speed = 0f;
        speed = agent.velocity.magnitude;
        animator.SetFloat("speed", speed);
    }

}
