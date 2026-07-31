using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemySO stats; 
    
    public NavMeshAgent agent;
    public Animator anim;
    public Transform player; 

    protected IAttackStrategy attackStrategy;

    protected IEnemyState currentState; 
    protected float currentHealth; 
    
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
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }

    protected abstract void SetInitialStrategy();

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"currentHealth");

        if (currentHealth <= 0)
        {
            //enemy.ChangeState(new DeadState(this))
        }
    }
    
    public void PerformAttack()
    {
        if (attackStrategy != null)
        {
            attackStrategy.ExecuteAttack(transform, player, stats.damage);
        }
    }

    public EnemySO GetStats()
    {
        return stats;
    }
    
    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }
}