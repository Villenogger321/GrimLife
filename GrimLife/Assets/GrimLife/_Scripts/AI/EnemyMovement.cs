using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float movementSpeed;
    [SerializeField] float detectionRadius, stopRadius, attackRadius;
    [SerializeField] EnemyAIState state;
    [SerializeField] float attackCooldown;
    float attackTimer;

    [Header("Wandering")]
    float curTimer;
    [SerializeField] float minWaitingTime, maxWaitingTime;
    [SerializeField] float wanderingRadius;
    [SerializeField] LayerMask groundMask;
    Vector3 startPos, wanderingDestination;

    public GameObject tempCube;

    Animator anim;
    NavMeshAgent agent;
    Transform player;

    void Start()
    {
        player = PlayerStats.Player;
        startPos = transform.position;
    }
    void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        switch (state)
        {
            case EnemyAIState.idle:
                HandleIdleState();
                CheckForAggroToPlayer();
                break;
            case EnemyAIState.wandering:
                HandleWanderingState();
                WalkTowards(wanderingDestination);
                CheckForAggroToPlayer();
                break;
            case EnemyAIState.chasing:
                HandleChasingState();
                break;
            case EnemyAIState.attacking:
                HandleAttackingState();
                break;
        }
        attackTimer -= Time.deltaTime;
    }
    void HandleIdleState()
    {
        curTimer -= Time.deltaTime;

        if (curTimer <= 0)
        {
            state = EnemyAIState.wandering;
            curTimer = Random.Range(minWaitingTime, maxWaitingTime);

            if (wanderingDestination == new Vector3())
            {
                wanderingDestination = startPos;
                return;
            }
            
            wanderingDestination = GetWanderingDestination();
        }
    }
    void HandleWanderingState()
    {
        if (Vector3.Distance(transform.position, wanderingDestination) <= 0.75f)
            state = EnemyAIState.idle;

        //Vector3 wanderingDir = wanderingDestination - transform.position;
    }
    void HandleChasingState()
    {
        if (Vector3.Distance(transform.position, player.position) < attackRadius)
        {
            state = EnemyAIState.attacking;

            if (Vector3.Distance(transform.position, player.position) < stopRadius)
            {
                if (attackTimer > 0)
                    state = EnemyAIState.idle;
                return;
            }
        }


        WalkTowards(player.position);

        Vector3 playerDir = player.position - transform.position;
    }
    void HandleAttackingState()
    {
        // start anim
        if (attackTimer > 0)
        {
            state = EnemyAIState.chasing;
            return;
        }

        attackTimer = attackCooldown;
    }
    void CheckForAggroToPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < detectionRadius)
        {
            state = EnemyAIState.chasing;
        }
    }
    void WalkTowards(Vector3 _destination)
    {
        agent.SetDestination(_destination);
        //transform.position += movementSpeed * Time.deltaTime * (_destination - transform.position).normalized;
    }
    Vector3 GetWanderingDestination()
    {
        Vector3 destination = new Vector3(Random.Range(-wanderingRadius, wanderingRadius), 20,
                                          Random.Range(-wanderingRadius, wanderingRadius));

        RaycastHit hit;
        Vector3 finalDestination = new Vector3();
        if (Physics.Raycast(startPos + destination, Vector3.down, out hit, Mathf.Infinity, groundMask))
        {
            Debug.DrawRay(destination, Vector3.down * hit.distance, Color.yellow);
            finalDestination = hit.point;
        }
        return finalDestination;
    }
    public float GetAttackRange()
    {
        return attackRadius;
    }
    void Event_AttackDone()
    {
        state = EnemyAIState.idle;
    }
    enum EnemyAIState
    {
        idle,
        wandering,
        chasing,
        attacking
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(Application.isPlaying ? startPos : transform.position, wanderingRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z), attackRadius);
    }
}
