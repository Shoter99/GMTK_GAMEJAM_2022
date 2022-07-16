using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Player : MonoBehaviour
{
    public int health = 10;

    public float moveSpeed = 1f;

    public int minValue;
    public int maxValue;
    public float moveDistance= 2;

    public Transform movePoint;

    [SerializeField]
    private string actionTaken = "None";

    private void Awake()
    {
        movePoint.parent = null;
    }

    private void Update()
    {
        if (GameManager.Instance.turn == "Enemies")
            return;

        switch (actionTaken)
        {
            case "None":
                return;

            case "Move":
                transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, movePoint.position) == 0f)
                {
                    if (moveDistance == 0)
                    {
                        GameManager.Instance.turn = "Enemies";
                        actionTaken = "None";
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
                break;
            case "Attack":

                if (Input.GetAxisRaw("Horizontal") == 1f)
                {
                    GameObject bullet = Instantiate(Addressables.LoadAssetAsync<GameObject>("Bullet").WaitForCompletion(), transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
                    bullet.GetComponent<BulletScript>().direction = "Right";
                    actionTaken = "None";
                    GameManager.Instance.turn = "Enemies";
                }

                if (Input.GetAxisRaw("Horizontal") == -1f)
                {
                    GameObject bullet = Instantiate(Addressables.LoadAssetAsync<GameObject>("Bullet").WaitForCompletion(), transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
                    bullet.GetComponent<BulletScript>().direction = "Left";
                    actionTaken = "None";
                    GameManager.Instance.turn = "Enemies";
                }

                if (Input.GetAxisRaw("Vertical") == 1f)
                {
                    GameObject bullet = Instantiate(Addressables.LoadAssetAsync<GameObject>("Bullet").WaitForCompletion(), transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
                    bullet.GetComponent<BulletScript>().direction = "Up";
                    actionTaken = "None";
                    GameManager.Instance.turn = "Enemies";
                }

                if (Input.GetAxisRaw("Vertical") == -1f)
                {
                    GameObject bullet = Instantiate(Addressables.LoadAssetAsync<GameObject>("Bullet").WaitForCompletion(), transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
                    bullet.GetComponent<BulletScript>().direction = "Down";
                    actionTaken = "None";
                    GameManager.Instance.turn = "Enemies";
                }
                break;
        }

        
    }

    public int SetMoveDistance(int minValue, int maxValue) => Random.Range(minValue, maxValue);

    public int TakeDamage(int health, int amount) => health -= amount;
}
