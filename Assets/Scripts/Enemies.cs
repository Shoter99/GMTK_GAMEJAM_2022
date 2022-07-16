using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public int health = 10;

    public float moveSpeed = 1f;

    public int minValue = 1;
    public int maxValue = 2;
    private float moveDistance;

    public bool isPlayerNear = false, isMoving = false;

    public Transform UpRaycast, DownRaycast, RightRaycast, LeftRaycast, movePoint;

    public Player player;

    public bool actionWaTaken = false;

    private void Start()
    {
        movePoint.parent = null;
        EnemyManager.Instance.enemies.Add(this);
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public bool IsPlayerNear(Transform UpRaycast, Transform DownRaycast, Transform RightRaycast, Transform LeftRaycast)
    {
        if (Physics2D.Raycast(UpRaycast.position, Vector3.up, 10))
            return true;

        if (Physics2D.Raycast(DownRaycast.position, Vector3.down, 10))
            return true;

        if (Physics2D.Raycast(RightRaycast.position, Vector3.right, 10))
            return true;

        if (Physics2D.Raycast(LeftRaycast.position, Vector3.left, 10))
            return true;

        return false;
    }

    private void FixedUpdate()
    {
        isPlayerNear = IsPlayerNear(UpRaycast, DownRaycast, RightRaycast, LeftRaycast);
    }

    public void TakeAction(int minValue, int maxValue, bool isPlayerNear, float moveSpeed, Transform movePoint)
    {
        moveDistance = Random.Range(minValue, maxValue);

        switch (Random.Range(0, 2))
        {
            case 0:
                if (isPlayerNear)
                {
                    player.health = player.TakeDamage(player.health, 1);
                }
                break;
            case 1:
                isMoving = true;
                StartCoroutine(Move(moveSpeed, movePoint));
                break;

        }
    }

    public IEnumerator Move(float moveSpeed, Transform movePoint)
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) == 0)
        {
            if (moveDistance == 0)
            {
                isMoving = false;
                yield break;
            }

            switch (Random.Range(1, 4))
            {
                case 1:
                    movePoint.transform.position += new Vector3(1, 0, 0);
                    break;
                case 2:
                    movePoint.transform.position += new Vector3(-1, 0, 0);
                    break;
                case 3:
                    movePoint.transform.position += new Vector3(0, 1, 0);
                    break;
                case 4:
                    movePoint.transform.position += new Vector3(0, -1, 0);
                    break;
            }

            moveDistance--;
        }

        yield return new WaitForEndOfFrame();
        StartCoroutine(Move(moveSpeed, movePoint));
    }

    private void OnDestroy()
    {
        EnemyManager.Instance.enemies.Remove(this);
    }
}
