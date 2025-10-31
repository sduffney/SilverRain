using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public GameObject targetPlayer;
    public NavMeshAgent agent;
    public abstract void Move();
    public abstract void Attack(Transform player);
}
