using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.animator.SetFloat("speed", enemy.patrolSpeed * 1.5f);
    }
    public override void UpdateState(EnemyStateManager enemy)
    {

        if (enemy.player == null)
        {
            enemy.SwitchState(enemy.idleState);
            return;
        }

        Vector3 toPlayer = enemy.player.position - enemy.transform.position;
        toPlayer.y = 0f; // Ignore vertical difference
        float distanceToPlayer = toPlayer.magnitude;

        // If player too far  go back to patrol
        if (distanceToPlayer > enemy.detectionRange)
        {
            enemy.SwitchState(enemy.patrolState);
            return;
        }

        // If close enough to attack  ENTER ATTACK STATE
        if (distanceToPlayer <= enemy.attackRange)
        {
            enemy.SwitchState(enemy.attackingState);
            return;
        }

        // Otherwise: move toward player
        Vector3 direction = toPlayer.normalized;
        enemy.transform.position += direction * (enemy.patrolSpeed * 3f) * Time.deltaTime;

        // Rotate to face player
        direction.y = 0f;
        if (direction.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(
                enemy.transform.rotation,
                targetRot,
                Time.deltaTime * 5f
            );
        }

    }
    public override void ExitState(EnemyStateManager enemy)
    {
        
        // Reset chase animation
        //enemy.GetComponent<Animator>().SetBool("isChasing", false);
    }

}
