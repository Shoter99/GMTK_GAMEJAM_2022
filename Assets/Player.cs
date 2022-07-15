using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 10;

    public float moveSpeed = 1f;

    public int minValue;
    public int maxValue;
    public float moveDistance= 2;

    public Transform movePoint;

    private void Awake()
    {
        movePoint.parent = null;
    }

    private void Update()
    {
        if (GameManager.Instance.turn == "Enemies")
            return;

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) == 0f)
        {
            if (moveDistance == 0)
            {
                GameManager.Instance.turn = "Enemies";
                moveDistance = SetMoveDistance(minValue, maxValue);
                return;
            }

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                moveDistance--;
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
            }

            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                moveDistance--;
                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
            }
        }
    }

    public int SetMoveDistance(int minValue, int maxValue) => Random.Range(minValue, maxValue);

    public void TakeDamage(int amount) => health -= amount;
}
