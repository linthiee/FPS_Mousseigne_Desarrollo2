using UnityEngine;

public interface IAttackStrategy
{
    void ExecuteAttack(Transform attacker, Transform player, float damage);
}

public class MeleeAttackStrategy : IAttackStrategy
{
    public void ExecuteAttack(Transform attacker, Transform player, float damage)
    {
        Debug.Log("melee");
    }
}

public class SniperAttackStrategy : IAttackStrategy
{
    public void ExecuteAttack(Transform attacker, Transform player, float damage)
    {
        Debug.Log("sniper");
    }
}

public class FastCloseShootStrategy : IAttackStrategy
{
    public void ExecuteAttack(Transform attacker, Transform player, float damage)
    {
        Debug.Log("close");
    }
}