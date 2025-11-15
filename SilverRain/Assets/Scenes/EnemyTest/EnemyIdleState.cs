using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Entering Idle State");
    }
    public override void UpdateState(EnemyStateManager enemy)
    {
        Debug.Log("Updating Idle State");
    }
    public override void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("Exiting Idle State");
    }

}
