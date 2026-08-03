using UnityEngine;

public interface IAttackStrategy
{
    void ExecuteAttack(Transform attacker, Transform player, float damage);
}

public class MeleeAttackStrategy : IAttackStrategy
{
    public void ExecuteAttack(Transform attacker, Transform player, float damage)
    {
        IDamageable target = player.GetComponent<IDamageable>();
        
        if (target != null)
        {
            target.TakeDamage(damage);
        }
    }
}

public class SniperAttackStrategy : IAttackStrategy
{
    public void ExecuteAttack(Transform attacker, Transform player, float damage)
    {
        IDamageable target = player.GetComponent<IDamageable>();
        
        if (target != null)
        {
            target.TakeDamage(damage);
        }    
    }
}

public class FastCloseShootStrategy : IAttackStrategy
{
    public void ExecuteAttack(Transform attacker, Transform player, float damage)
    {
        IDamageable target = player.GetComponent<IDamageable>();
        
        if (target != null)
        {
            target.TakeDamage(damage);
        }        }
}