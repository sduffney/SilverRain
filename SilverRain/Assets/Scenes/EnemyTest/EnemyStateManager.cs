using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // declare states
    EnemyBaseState currentState;
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyPatrolState patrolState = new EnemyPatrolState();
    public EnemyAttackingState attackingState = new EnemyAttackingState();
    public EnemyChaseState chaseState = new EnemyChaseState();
    public EnemyDeadState deadState = new EnemyDeadState();
    public EnemySenseState senseState = new EnemySenseState();

    // animator reference
    public Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        currentState = idleState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }
    public void SwitchState(EnemyBaseState state)
    {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }
}
