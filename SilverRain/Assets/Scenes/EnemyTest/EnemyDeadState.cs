using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Entering Dead State");
    }
    public override void UpdateState(EnemyStateManager enemy)
    {
        Debug.Log("Updating Dead State");
    }
    public override void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("Exiting Dead State");
    }

}
