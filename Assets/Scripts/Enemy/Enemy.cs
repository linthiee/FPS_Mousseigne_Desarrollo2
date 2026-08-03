using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected EnemySO stats;

    [SerializeField] public AudioSource audioSource;

    public NavMeshAgent agent;
    public Animator anim;
    public Transform player;

    protected IAttackStrategy attackStrategy;

    protected IEnemyState currentState;
    protected float currentHealth;

    public bool isDead = false;

    protected virtual void Awake()
    {
        Debug.Log("me desperte");
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (GameObject.FindGameObjectWithTag("Player") != null)
            Debug.Log("chasing");
        else
            Debug.Log("NOT chasing");

        agent.speed = stats.moveSpeed;
        agent.stoppingDistance = stats.stoppingDistance;
        currentHealth = stats.maxHealth;

        SetInitialStrategy();
    }

    protected virtual void Start()
    {
        ChangeState(new IdleState(this));
    }

    protected virtual void Update()
    {
        if (!isDead)
        {
            if (currentState != null)
            {
                currentState.UpdateState();
            }
        }
    }

    protected abstract void SetInitialStrategy();

    public void TakeDamage(float amount)
    {
        if (!isDead)
        {
            currentHealth -= amount;
            ChangeState(new HitState(this));

            if (currentHealth <= 0)
            {
                ChangeState(new DeadState(this));
            }
        }
    }

    public void PerformAttack()
    {
        if (!isDead)
        {
            if (attackStrategy != null)
            {
                attackStrategy.ExecuteAttack(transform, player, stats.damage);
            }
        }
    }

    public EnemySO GetStats()
    {
        return stats;
    }

    public void ChangeState(IEnemyState newState)
    {
        if (!isDead)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = newState;
            currentState.Enter();
        }
    }
}