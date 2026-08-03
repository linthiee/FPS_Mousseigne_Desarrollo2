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

public class HitState : IEnemyState
{
    private Enemy enemy;

    private readonly int hitHash = Animator.StringToHash("Get_Hit");
    private float hitTimer;

    private float stunDuration = 0.5f;
    private float audioCooldown;

    public HitState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.agent.ResetPath();

        enemy.anim.CrossFade(hitHash, 0.2f);

        hitTimer = stunDuration;
    }

    public void UpdateState()
    {
        if (audioCooldown > 0f)
        {
            audioCooldown -= Time.deltaTime;
        }

        hitTimer -= Time.deltaTime;
        
        if (hitTimer <= 0f && audioCooldown <= 0f)
        {
            enemy.audioSource.PlayOneShot(enemy.GetStats().hit);
            audioCooldown = 0.5f;
        }        
        
        if (hitTimer <= 0f)
        {
            float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);

            if (distance <= enemy.GetStats().attackRange)
            {
                enemy.agent.SetDestination(enemy.player.position);
                enemy.ChangeState(new AttackState(enemy));
            }
            else if (distance <= enemy.GetStats().range)
            {
                enemy.ChangeState(new ChaseState(enemy));
            }
            else
            {
                enemy.ChangeState(new IdleState(enemy));
            }
        }
    }

    public void Exit()
    {
    }
}

public class DeadState : IEnemyState
{
    private Enemy enemy;

    private readonly int deadHash = Animator.StringToHash("Dead");

    public DeadState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.audioSource.PlayOneShot(enemy.GetStats().dead);
        enemy.anim.CrossFade(deadHash, 0.2f);
        enemy.isDead = true;
    }

    public void UpdateState()
    {
    }

    public void Exit()
    {
    }
}

public class AttackState : IEnemyState
{
    private Enemy enemy;
    private float attackTimer;

    private int[] attackHashes;

    private readonly int idleHash = Animator.StringToHash("Idle");

    private bool isWaiting;

    public AttackState(Enemy enemy)
    {
        this.enemy = enemy;
        
        string[] animNames = enemy.GetStats().attackAnimations;
        attackHashes = new int[animNames.Length];
        
        for (int i = 0; i < animNames.Length; i++)
        {
            attackHashes[i] = Animator.StringToHash(animNames[i]);
        }
    }

    public void Enter()
    {
        enemy.agent.ResetPath();

        int randomIndex = Random.Range(0, attackHashes.Length);
        int randomAttack = attackHashes[randomIndex];

        enemy.anim.CrossFade(randomAttack, 0.1f);
        enemy.audioSource.PlayOneShot(enemy.GetStats().attack);

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

        if (distance <= enemy.GetStats().range)
        {
            enemy.ChangeState(new ChaseState(enemy));
        }
    }

    public void Exit()
    {
    }
}