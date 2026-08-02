using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public interface IEnemyState
{
    void Enter();
    void UpdateState();
    void Exit();
}

public class ChaseState : IEnemyState
{
    private Enemy enemy;
    
    private readonly int walkHash = Animator.StringToHash("Walk");

    public ChaseState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.audioSource.Play();
        enemy.anim.CrossFade(walkHash, 0.2f); 
    }

    public void UpdateState()
    {
        enemy.agent.SetDestination(enemy.player.position);

        float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);
        if (distance <= enemy.GetStats().attackRange)
        {
            enemy.ChangeState(new AttackState(enemy));
        }
    }

    public void Exit()
    {
        enemy.audioSource.Stop();
    }
}

public class AttackState : IEnemyState
{
    private Enemy enemy;
    private float attackTimer;

    private readonly int[] attackHashes = new int[]
    {
        Animator.StringToHash("Attack(1)"),
        Animator.StringToHash("Attack(2)"),
        Animator.StringToHash("Attack(3)")
    };

    private readonly int idleHash = Animator.StringToHash("Idle");

    private bool isWaiting;

    public AttackState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.agent.ResetPath();

        int randomIndex = Random.Range(0, attackHashes.Length);
        int randomAttack = attackHashes[randomIndex];

        enemy.anim.CrossFade(randomAttack, 0.1f);
        enemy.audioSource.PlayOneShot(enemy.stats.attack);

        attackTimer = enemy.GetStats().attackCooldown;
        isWaiting = false;
    }

    public void UpdateState()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= (enemy.GetStats().attackCooldown - 1f) && !isWaiting)
        {
            enemy.anim.CrossFade(idleHash, 0.2f);
            isWaiting = true;
        }

        if (attackTimer <= 0f)
        {
            float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);

            if (distance > enemy.GetStats().attackRange)
            {
                enemy.ChangeState(new ChaseState(enemy));
            }
            else
            {
                enemy.PerformAttack();
                
                int randomIndex = Random.Range(0, attackHashes.Length);
                int randomAttack = attackHashes[randomIndex];

                enemy.anim.CrossFade(randomAttack, 0.1f, -1, 0f);
                attackTimer = enemy.GetStats().attackCooldown;
                
                isWaiting = false;
            }
        }
    }
    
    public void Exit()
    {
        
    }
}

public class IdleState : IEnemyState
{
    private Enemy enemy;
    private readonly int idleHash = Animator.StringToHash("Idle");

    public IdleState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.agent.ResetPath(); 
        
        enemy.anim.CrossFade(idleHash, 0.2f);
    }

    public void UpdateState()
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);
        
        if (distance <= 15f) 
        {
            enemy.ChangeState(new ChaseState(enemy));
        }
    }
    
    public void Exit()
    {
        
    }
}