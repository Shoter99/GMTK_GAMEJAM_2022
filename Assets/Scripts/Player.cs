using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public sealed class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public int health = 10;

    public bool valueIsRolled = false, bulletExists = false;

    [SerializeField]
    [Range(0, 20)]
    private float moveSpeed = 1f;

    public int minValue, maxValue, valueRolled;

    [SerializeField]
    private string actionTaken = "None";

    [SerializeField]
    private List<Transform> raycasts = new List<Transform>();

    private Transform movePoint;

    private void Awake()
    {
        movePoint = transform.GetChild(0).transform;
        movePoint.parent = null;
    }

    private void Update()
    {
        if (GameManager.Instance.turn == "Enemies")
            return;

        if (!valueIsRolled)
        {
            valueIsRolled = true;
            valueRolled = RollAValue(minValue, maxValue);
        }

        switch (actionTaken)
        {
            case "None":
                return;

            case "Move":
                Move();
                break;
            case "Fire":
                Fire();               
                break;
            case "Melee":
                MeleeAttack();
                break;
            case "Heal":
                health = Heal(health);
                valueRolled = 0;
                valueIsRolled = false;
                actionTaken = "None";
                GameManager.Instance.turn = "Enemies";
                break;
            case "Defend":
                Defend();
                break;
        }       
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) == 0f)
        {
            if (valueRolled == 0)
            {
                valueIsRolled = false;
                GameManager.Instance.turn = "Enemies";
                actionTaken = "None";
                return;
            }

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                valueRolled--;
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
            }

            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                valueRolled--;
                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
            }
        }
    }

    private void Fire()
    {
        if (Input.GetAxisRaw("Horizontal") == 1f)
        {
            GameObject bullet = Instantiate(Addressables.LoadAssetAsync<GameObject>("Bullet").WaitForCompletion(), transform.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().direction = "Right";
            bullet.GetComponent<BulletScript>().strength = valueRolled;
            bullet.GetComponent<BulletScript>().owner = gameObject;
            actionTaken = "None";
            valueRolled = 0;
            valueIsRolled = false;
            bulletExists = true;
            StartCoroutine(WaitForBulletToDestroy());
        }

        if (Input.GetAxisRaw("Horizontal") == -1f)
        {
            GameObject bullet = Instantiate(Addressables.LoadAssetAsync<GameObject>("Bullet").WaitForCompletion(), transform.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().direction = "Left";
            bullet.GetComponent<BulletScript>().strength = valueRolled;
            bullet.GetComponent<BulletScript>().owner = gameObject;
            actionTaken = "None";
            valueRolled = 0;
            valueIsRolled = false;
            bulletExists = true;
            StartCoroutine(WaitForBulletToDestroy());
        }

        if (Input.GetAxisRaw("Vertical") == 1f)
        {
            GameObject bullet = Instantiate(Addressables.LoadAssetAsync<GameObject>("Bullet").WaitForCompletion(), transform.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().direction = "Up";
            bullet.GetComponent<BulletScript>().strength = valueRolled;
            bullet.GetComponent<BulletScript>().owner = gameObject;
            actionTaken = "None";
            valueRolled = 0;
            valueIsRolled = false;
            bulletExists = true;
            StartCoroutine(WaitForBulletToDestroy());
        }

        if (Input.GetAxisRaw("Vertical") == -1f)
        {
            GameObject bullet = Instantiate(Addressables.LoadAssetAsync<GameObject>("Bullet").WaitForCompletion(), transform.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().direction = "Down";
            bullet.GetComponent<BulletScript>().strength = valueRolled;
            bullet.GetComponent<BulletScript>().owner = gameObject;
            actionTaken = "None";
            valueRolled = 0;
            valueIsRolled = false;
            bulletExists = true;
            StartCoroutine(WaitForBulletToDestroy());
        }
    }

    private void MeleeAttack()
    {
        if (Input.GetAxisRaw("Horizontal") == 1f)
        {
            RaycastHit2D hit = Physics2D.Raycast(raycasts[2].position, Vector3.right, 1);

            if (hit)
            {
                hit.collider.gameObject.GetComponent<Enemies>().health = hit.collider.gameObject.GetComponent<Enemies>().TakeDamage(hit.collider.gameObject.GetComponent<Enemies>().health, valueRolled);
            }

            valueRolled = 0;
            valueIsRolled = false;
            actionTaken = "None";
            GameManager.Instance.turn = "Enemies";
        }

        if (Input.GetAxisRaw("Horizontal") == -1f)
        {
            RaycastHit2D hit = Physics2D.Raycast(raycasts[3].position, Vector3.left, 1);

            if (hit)
            {
                hit.collider.gameObject.GetComponent<Enemies>().health = hit.collider.gameObject.GetComponent<Enemies>().TakeDamage(hit.collider.gameObject.GetComponent<Enemies>().health, valueRolled);
            }

            valueRolled = 0;
            valueIsRolled = false;
            actionTaken = "None";
            GameManager.Instance.turn = "Enemies";
        }

        if (Input.GetAxisRaw("Vertical") == 1f)
        {
            RaycastHit2D hit = Physics2D.Raycast(raycasts[0].position, Vector3.up, 1);

            if (hit)
            {
                hit.collider.gameObject.GetComponent<Enemies>().health = hit.collider.gameObject.GetComponent<Enemies>().TakeDamage(hit.collider.gameObject.GetComponent<Enemies>().health, valueRolled);
            }

            valueRolled = 0;
            valueIsRolled = false;
            actionTaken = "None";
            GameManager.Instance.turn = "Enemies";
        }

        if (Input.GetAxisRaw("Vertical") == -1f)
        {
            RaycastHit2D hit = Physics2D.Raycast(raycasts[1].position, Vector3.down, 1);

            if (hit)
            {
                hit.collider.gameObject.GetComponent<Enemies>().health = hit.collider.gameObject.GetComponent<Enemies>().TakeDamage(hit.collider.gameObject.GetComponent<Enemies>().health, valueRolled);
            }

            valueRolled = 0;
            valueIsRolled = false;
            actionTaken = "None";
            GameManager.Instance.turn = "Enemies";
        }
    }
    
    private void Defend()
    {
        actionTaken = "None";
        GameManager.Instance.turn = "Enemies";
        valueRolled = 0;
        valueIsRolled = false;
    }

    private int Heal(int health) => health + valueRolled;

    public int RollAValue(int minValue, int maxValue) => Random.Range(minValue, maxValue + 1);

    public int TakeDamage(int health, int amount) => health -= amount;


    private IEnumerator WaitForBulletToDestroy()
    {
        yield return new WaitForEndOfFrame();
        
        if (!bulletExists)
        {
            GameManager.Instance.turn = "Enemies";
            yield break;
        }

        StartCoroutine(WaitForBulletToDestroy());
    }
}
