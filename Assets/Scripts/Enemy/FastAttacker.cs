public class FastAttacker : Enemy
{
    protected override void SetInitialStrategy()
    {
        attackStrategy = new FastCloseShootStrategy();
    }
}