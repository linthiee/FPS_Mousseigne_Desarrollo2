public class Melee : Enemy
{
    protected override void SetInitialStrategy()
    {
        attackStrategy = new MeleeAttackStrategy();
    }
}