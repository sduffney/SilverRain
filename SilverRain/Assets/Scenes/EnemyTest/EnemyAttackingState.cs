using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Entering Attacking State");
    }
    public override void UpdateState(EnemyStateManager enemy)
    {
        Debug.Log("Updating Attacking State");
    }
    public override void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("Exiting Attacking State");
    }



}
