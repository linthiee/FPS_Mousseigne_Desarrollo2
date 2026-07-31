public class SlowAttacker : Enemy
{
    protected override void SetInitialStrategy()
    {
        attackStrategy = new SniperAttackStrategy();
    }
}