using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float movementSpeed;
    [SerializeField] float detectionRadius, stopRadius, attackRadius;
    [SerializeField] EnemyAIState state;
    [SerializeField] float attackCooldown;
    bool addedToFight;
    float attackTimer;

    [Header("Wandering")]
    float curTimer;
    [SerializeField] float minWaitingTime, maxWaitingTime;
    [SerializeField] float wanderingRadius;
    [SerializeField] LayerMask groundMask;
    Vector3 startPos, wanderingDestination;

    Animator anim;
    NavMeshAgent agent;
    Transform player;
    AIBiasValues aiBiasValues;

    void Start()
    {
        player = PlayerStats.Player.transform;
        startPos = transform.position;
    }
    void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        aiBiasValues = GetComponent<AIBiasValues>();
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
            case EnemyAIState.prepareAttack:
                HandlePrepareAttackState();
                break;
            case EnemyAIState.attacking:
                HandleAttackingState();
                break;
            case EnemyAIState.backing:
                HandleBackingState();
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
    }
    void HandleChasingState()
    {
        Debug.Log("Enemy Chasing Animation");
        anim.SetBool("Chase", true);
        if (playerDistance() < stopRadius)
        {
            //state = EnemyAIState.attacking;
            anim.SetBool("Chase", false);
            return;
        }
        WalkTowards(player.position);
    }
    void HandlePrepareAttackState()
    {
        if (playerDistance() < attackRadius)
        {
            state = EnemyAIState.attacking;
            return;
        }
        WalkTowards(player.position);
    }
    void HandleAttackingState()
    {
        Debug.Log("HandleAttackingState");
        if (attackTimer <= 0)
        {
            aiBiasValues.SetAttackTimestamp(Time.time);
            attackTimer = attackCooldown;
        }
        state = EnemyAIState.backing;

        Debug.Log("Enemy Attacking Animation");
        anim.SetTrigger("Attack");

    }
    void HandleBackingState()
    {
        if (attackTimer <= 0)
        {
            state = EnemyAIState.chasing;
            return;
        }

        Vector3 dir = (transform.position - player.position).normalized;
        Vector3 destination = player.position + (dir * stopRadius);

        transform.rotation = Quaternion.LookRotation(new Vector3(-dir.x, 0, -dir.z));


        WalkTowards(destination, 0);
    }
    void CheckForAggroToPlayer()
    {
        if (playerDistance() < detectionRadius)
        {
            state = EnemyAIState.chasing;
            AIManager.AddAggroedEnemy(this);
        }
    }
    void WalkTowards(Vector3 _destination, int _angularSpeed = 120)
    {
        agent.angularSpeed = _angularSpeed;
        agent.SetDestination(_destination);
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
    void OnDisable()
    {
        AIManager.RemoveAggroedEnemy(this);

        return;
    }
    public void SignalPrepareAttack()
    {
        if (state == EnemyAIState.attacking)
            return;

        state = EnemyAIState.prepareAttack;
    }
    public int GetBiasValue()
    {
        if (state == (EnemyAIState.attacking | EnemyAIState.prepareAttack))
        {
            Debug.Log("zeo");
            return 0;
        }

        return aiBiasValues.CalculateBias();
    }

    float playerDistance()
    {
        return Vector3.Distance(transform.position, player.position);
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
        prepareAttack,
        attacking,
        backing
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(Application.isPlaying ? startPos : transform.position, wanderingRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, stopRadius);
    }
}
