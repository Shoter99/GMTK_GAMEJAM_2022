using System.Collections;
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

    private GameObject player;

    private void Awake()
    {
        movePoint.parent = null;
        EnemyManager.Instance.enemies.Add(this);
    }

    public bool IsPlayerNear(Transform UpRaycast, Transform DownRaycast, Transform RightRaycast, Transform LeftRaycast)
    {
        if (Physics.Raycast(UpRaycast.position, Vector3.up, 1))
            return true;

        if (Physics.Raycast(DownRaycast.position, Vector3.down, 1))
            return true;

        if (Physics.Raycast(RightRaycast.position, Vector3.right, 1))
            return true;

        if (Physics.Raycast(LeftRaycast.position, Vector3.left, 1))
            return true;

        return false;
    }

    public void TakeAction(int minValue, int maxValue, bool isPlayerNear, float moveSpeed, Transform movePoint)
    {
        moveDistance = Random.Range(minValue, maxValue);

        switch (1)
        {
            case 0:
                if (isPlayerNear)
                {
                    player.GetComponent<Player>().TakeDamage(1);
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
