using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyController : EnemyController
{
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    private void OnCollisionStay(Collision collision)
    {
        //Attack
        //Add timer to deal damage
    }

    private void Update()
    {
        Move();
    }
    public override void Attack(Transform player)
    {
        //Deal damage
    }

    public override void Move()
    {
        //Move towards the player
        agent.SetDestination(targetPlayer.transform.position);
    }
}
