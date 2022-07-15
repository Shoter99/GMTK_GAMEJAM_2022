using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 10;

    public float moveSpeed = 0.2f;

    public int minValue = 1;
    public int maxValue = 2;
    private float moveDistance;

    public Transform movePoint;

    private void Update()
    {
        if (GameManager.Instance.turn == "Enemies")
            return;

        if (moveDistance == 0)
            SetMoveDistance(minValue, maxValue);

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if (moveDistance == 0)
            {
                GameManager.Instance.turn = "Enemies";
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
                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Horizontal"), 0f);
            }
        }
    }

    public void SetMoveDistance(int minValue, int maxValue) => moveDistance = Random.Range(minValue, maxValue);

    public void TakeDamage(int amount) => health -= amount;
}
