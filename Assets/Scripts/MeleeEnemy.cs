using UnityEngine;
using UnityEngine.AddressableAssets;

public sealed class MeleeEnemy : Enemies
{
    public string wherePlayer;

    public GameObject newMelee;

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

        hit = Physics2D.Raycast(raycasts[0].position, Vector3.up, raycastLength - 0.5f);

        if (hit)
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                wherePlayer = "Up";
                return true;
            }

        hit = Physics2D.Raycast(raycasts[1].position, Vector3.down, raycastLength - 0.5f);

        if (hit)
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                wherePlayer = "Down";
                return true;
            }

        hit = Physics2D.Raycast(raycasts[2].position, Vector3.right, raycastLength - 0.5f);

        if (hit)
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                wherePlayer = "Right";
                return true;
            }

        hit = Physics2D.Raycast(raycasts[3].position, Vector3.left, raycastLength - 0.5f);

        if (hit)
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                wherePlayer = "Left";
                return true;
            }

        return false;
    }

    public override void Attack()
    {
        base.Attack();
        if (IsPlayerNear())
        {
            GameObject melee = Instantiate(newMelee, transform.position, Quaternion.identity);
            melee.GetComponent<BulletScript>().strength = storedValue;
            melee.GetComponent<BulletScript>().length = 1;
            melee.GetComponent<BulletScript>().owner = gameObject;
            melee.GetComponent<BulletScript>().direction = wherePlayer;
            bulletExists = true;
;
        }
    }
}
