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

    [Header("Idle State Settings")]
    [Range(.5f, 10f)]
    [SerializeField] public float idleDuration = 2f; // center value for idle duration
    [Range(0f, 1f)]
    [SerializeField] public float idleDurationVariance = 0.2f; // variance percentage for idle duration


    [Header("Patrol State Settings")]
    [SerializeField] public float patrolRadius = 5f;
    [SerializeField] public float patrolSpeed = 2f;
    [HideInInspector] public Vector3 patrolPoint;


    [Header("Combat Settings")]
    public Transform player;
    public float detectionRange = 20f;
    public float attackRange = 10f;
    public int attackDamage = 1;




    void Awake()
    {
        animator = GetComponent<Animator>();
        patrolPoint = transform.position;
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

    // Animation Event Bridges
    public void OnAttackHit()
    {
        if (currentState is EnemyAttackingState attacking)
        {
            attacking.OnAttackHit(this);
        }
    }
    public void AttackAnimationEnd()
    {
        if (currentState is EnemyAttackingState attacking)
        {
            attacking.AttackAnimationEnd(this);
        }
    }

}
