public sealed class MeleeEnemy : Enemies
{

    public override void Attack()
    {
        base.Attack();
        if (IsPlayerNear(raycasts[0], raycasts[1], raycasts[2], raycasts[3]))
        {
            player.health = player.TakeDamage(player.health, 1);
        }
    }
}
