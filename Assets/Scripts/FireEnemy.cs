using UnityEngine;
using UnityEngine.AddressableAssets;

public class FireEnemy : Enemies
{
    public bool bulletFiredThisTurn = false;

    private string playerPosition;

    public GameObject newBullet;

    private void Start()
    {
        EnemyManager.Instance.rangeEnemies.Add(this);
    }

    private void OnDestroy()
    {
        EnemyManager.Instance.rangeEnemies.Remove(this);
    }

    private bool IsPlayerInRange()
    {
        RaycastHit2D hit;

        hit = Physics2D.Raycast(raycasts[0].position, Vector3.up, rolledValue - 0.5f);

        if (hit)
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                playerPosition = "Up";
                return true;
            }

        hit = Physics2D.Raycast(raycasts[1].position, Vector3.down, rolledValue - 0.5f);

        if (hit)
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                playerPosition = "Down";
                return true;
            }

        hit = Physics2D.Raycast(raycasts[2].position, Vector3.right, rolledValue - 0.5f);

        if (hit)
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                playerPosition = "Right";
                return true;
            }

        hit = Physics2D.Raycast(raycasts[3].position, Vector3.left, rolledValue - 0.5f);

        if (hit)
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                playerPosition = "Left";
                return true;
            }

        return false;
    }

    public override void Attack()
    {
        base.Attack();

        if (!IsPlayerInRange())
            return;

        bulletExists = true;
        bulletFiredThisTurn = true;
        GameObject bullet = Instantiate(newBullet, transform.position, Quaternion.identity);

        switch (playerPosition)
        {
            case "Up":
                bullet.GetComponent<BulletScript>().direction = "Up";
                break;
            case "Down":
                bullet.GetComponent<BulletScript>().direction = "Down";
                break;
            case "Right":
                bullet.GetComponent<BulletScript>().direction = "Right";
                break;
            case "Left":
                bullet.GetComponent<BulletScript>().direction = "Left";
                break;
        }

        bullet.GetComponent<BulletScript>().strength = rolledValue;
        bullet.GetComponent<BulletScript>().owner = gameObject;
    }
}
