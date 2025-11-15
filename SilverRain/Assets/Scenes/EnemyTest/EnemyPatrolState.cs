using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Entering Patrol State");
        // Initialize patrol behavior, set waypoints, etc.
    }
    public override void UpdateState(EnemyStateManager enemy)
    {
        // Implement patrol logic, move between waypoints
        Debug.Log("Patrolling...");
        
        // Example transition condition
        //if (Vector3.Distance(enemy.transform.position, enemy.player.position) < enemy.detectionRange)
        //{
        //    enemy.SwitchState(enemy.senseState);
        //}
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("Exiting Patrol State");
        // Clean up patrol behavior if necessary
    }


}
