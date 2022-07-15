using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemies : MonoBehaviour
{
    private GameObject player;

    private bool IsPlayerNear(Transform UpRaycast, Transform DownRaycast, Transform RightRaycast, Transform LeftRaycast)
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

    public void TakeAction(int minValue, int maxValue, bool isPlayerNear, int moveSpeed, Transform movePoint)
    {
        int value = Random.Range(minValue, maxValue);

        switch (Random.Range(0, 1))
        {
            case 0:
                if (isPlayerNear)
                {

                }
                break;
            case 1:
                Move(moveSpeed, value, movePoint);
                break;

        }
    }

    public void Move(int moveSpeed, int moveDistance, Transform movePoint)
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) == 0)
        {
            if (moveDistance == 0)
                return;

            switch (Random.Range(1, 4))
            {
                case 1:
                    transform.position += new Vector3(1, 0, 0);
                    break;
                case 2:
                    transform.position += new Vector3(-1, 0, 0);
                    break;
                case 3:
                    transform.position += new Vector3(0, 1, 0);
                    break;
                case 4:
                    transform.position += new Vector3(0, -1, 0);
                    break;
            }

            moveDistance--;
        }
    }
}
