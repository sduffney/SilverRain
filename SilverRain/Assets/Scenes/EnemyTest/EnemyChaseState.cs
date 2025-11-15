using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Entering Chase State");
        // Set chase animation
        //enemy.GetComponent<Animator>().SetBool("isChasing", true);
    }
    public override void UpdateState(EnemyStateManager enemy)
    {
        // Chase logic here
        Debug.Log("Chasing the player...");
        // Example condition to switch to attacking state
        //float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.position);
        //if (distanceToPlayer < enemy.attackRange)
        //{
        //    enemy.SwitchState(enemy.attackingState);
        //}
    }
    public override void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("Exiting Chase State");
        // Reset chase animation
        //enemy.GetComponent<Animator>().SetBool("isChasing", false);
    }

}
