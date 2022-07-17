using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public abstract class Enemies : MonoBehaviour
{
    public int health = 10;

    public int minValue, maxValue, rolledValue;

    private int stackOverFlowProtection = 0;

    [Range(0, 20)]
    public float moveSpeed;

    public bool isMoving = false;


    public float raycastLength;

    [HideInInspector]
    public Transform movePoint;

    [HideInInspector]
    public List<Transform> raycasts = new List<Transform>();

    [HideInInspector]
    public Player player;

    private void Awake()
    {
        movePoint = transform.GetChild(0);
        movePoint.parent = null;
        player = GameObject.Find("Player").GetComponent<Player>();

        for (int i = 0; i < 4; i++)
        {
            raycasts.Add(transform.GetChild(i).transform);
        }
    }

    public virtual void Attack()
    {
        
    }

    private void LateUpdate()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) == 0)
        {
            if (rolledValue == 0)
            {
                isMoving = false;
                yield break;
            }

            RaycastHit2D hit;

            switch (Random.Range(1, 5))
            {
                case 1:
                    hit = Physics2D.Raycast(raycasts[2].position, Vector2.right, raycastLength - 0.5f);
                    if (hit)
                    {
                        stackOverFlowProtection++;
                        if (stackOverFlowProtection >= 50)
                        {
                            isMoving = false;
                            yield break;
                        }
                        StartCoroutine(Move());
                        yield break;
                    }                 
                    movePoint.transform.position += new Vector3(1, 0, 0);
                    break;
                case 2:
                    hit = Physics2D.Raycast(raycasts[3].position, Vector2.left, raycastLength - 0.5f);
                    if (hit)
                    {
                        stackOverFlowProtection++;
                        if (stackOverFlowProtection >= 50)
                        {
                            isMoving = false;
                            yield break;
                        }
                        StartCoroutine(Move());
                        yield break;
                    }
                    movePoint.transform.position += new Vector3(-1, 0, 0);
                    break;
                case 3:
                    hit = Physics2D.Raycast(raycasts[0].position, Vector2.up, raycastLength - 0.5f);
                    if (hit)
                    {
                        stackOverFlowProtection++;
                        if (stackOverFlowProtection >= 50)
                        {
                            isMoving = false;
                            yield break;
                        }
                        StartCoroutine(Move());
                        yield break;
                    }
                    movePoint.transform.position += new Vector3(0, 1, 0);
                    break;
                case 4:
                    hit = Physics2D.Raycast(raycasts[1].position, Vector2.down, raycastLength - 0.5f);
                    if (hit)
                    {
                        stackOverFlowProtection++;
                        if (stackOverFlowProtection >= 50)
                        {
                            isMoving = false;
                            yield break;
                        }
                        StartCoroutine(Move());
                        yield break;
                    }
                    movePoint.transform.position += new Vector3(0, -1, 0);
                    break;
            }

            rolledValue--;
        }

        stackOverFlowProtection = 0;
        yield return new WaitForEndOfFrame();
        StartCoroutine(Move());
    }

    public int TakeDamage(int health, int amount) => health -= amount;

    public int RollNumber(int minValue, int maxValue) => Random.Range(minValue, maxValue + 1);
}
