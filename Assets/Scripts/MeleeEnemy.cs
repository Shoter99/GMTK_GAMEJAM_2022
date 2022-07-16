using UnityEngine;

public sealed class MeleeEnemy : Enemies
{
    private void Start()
    {
        EnemyManager.Instance.meleeEnemies.Add(this);
    }

    private void OnDestroy()
    {
        EnemyManager.Instance.meleeEnemies.Remove(this);
    }

    private bool IsPlayerNear()
    {
        RaycastHit2D hit;

        hit = Physics2D.Raycast(raycasts[0].position, Vector3.up, raycastLength);

        if (hit)
            if (hit.collider.gameObject.CompareTag("Player"))
                return true;

        hit = Physics2D.Raycast(raycasts[1].position, Vector3.down, raycastLength);

        if (hit)
            if (hit.collider.gameObject.CompareTag("Player"))
                return true;

        hit = Physics2D.Raycast(raycasts[2].position, Vector3.right, raycastLength);

        if (hit)
            if (hit.collider.gameObject.CompareTag("Player"))
                return true;

        hit = Physics2D.Raycast(raycasts[3].position, Vector3.left, raycastLength);

        if (hit)
            if (hit.collider.gameObject.CompareTag("Player"))
                return true;

        return false;
    }

    public override void Attack()
    {
        base.Attack();
        if (IsPlayerNear())
        {
            player.TakeDamage(rolledValue);
        }
    }
}
