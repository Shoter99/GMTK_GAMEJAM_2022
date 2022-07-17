using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public sealed class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Range(0, 20)]
    public int health = 15;

    public int actionsLeft;

    [HideInInspector]
    public int damageResistanceStrength = 0, shieldDurability = 0;

    public bool valueIsRolled = false, bulletExists = false;

    [SerializeField]
    [Range(0, 20)]
    private float moveSpeed = 1f;

    public int minValue, maxValue, valueRolled;

    [SerializeField]
    private string actionTaken = "None";

    private readonly List<Transform> raycasts = new List<Transform>();

    private Transform movePoint;


    private void Awake()
    {
        Instance = this;
        movePoint = transform.GetChild(0).transform;
        movePoint.parent = null;

        for (int i = 0; i < 4; i++)
        {
            raycasts.Add(transform.GetChild(i).transform);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.turn == "Enemies")
            return;

        if (bulletExists)
            return;

        if (!valueIsRolled)
        {
            valueIsRolled = true;
            valueRolled = RollAValue(minValue, maxValue);
        }

        switch (actionTaken)
        {
            case "None":
                if (actionsLeft <= 0)
                    GameManager.Instance.turn = "Enemies";
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
                Heal();
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
                PrepereForNextAction();
                return;
            }

            RaycastHit2D hit;

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                switch (Input.GetAxisRaw("Horizontal"))
                {
                    case 1f:
                        hit = Physics2D.Raycast(raycasts[2].position, Vector2.right, 0.5f);

                        if (hit)
                        {
                            if (!hit.collider.CompareTag("Enemy") && !hit.collider.CompareTag("Walls"))
                            {
                                valueRolled--;
                                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                            }
                        }
                        else
                        {
                            valueRolled--;
                            movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                        }
                        break;

                    case -1f:
                        hit = Physics2D.Raycast(raycasts[3].position, Vector2.left, 0.5f);
                        if (hit)
                        {
                            if (!hit.collider.CompareTag("Enemy") && !hit.collider.CompareTag("Walls"))
                            {
                                valueRolled--;
                                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                            }
                        }
                        else
                        {
                            valueRolled--;
                            movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                        }
                        break;
                }
            }

            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                switch (Input.GetAxisRaw("Vertical"))
                {
                    case 1f:
                        hit = Physics2D.Raycast(raycasts[0].position, Vector2.up, 0.5f);
                        if (hit)
                        {
                            if (!hit.collider.CompareTag("Enemy") && !hit.collider.CompareTag("Walls"))
                            {
                                valueRolled--;
                                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                            }
                        }
                        else
                        {
                            valueRolled--;
                            movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                        }
                        break;

                    case -1f:
                        hit = Physics2D.Raycast(raycasts[1].position, Vector2.down, 0.5f);
                        if (hit)
                        {
                            if (!hit.collider.CompareTag("Enemy") && !hit.collider.CompareTag("Walls"))
                            {
                                valueRolled--;
                                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                            }
                        }
                        else
                        {
                            valueRolled--;
                            movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                        }
                        break;
                }
            }
        }
    }

    private void Fire()
    {
        if (Input.GetAxisRaw("Horizontal") == 1f)
        {
            SpawnBullet().GetComponent<BulletScript>().direction = "Right";
            PrepereForNextAction();
        }

        if (Input.GetAxisRaw("Horizontal") == -1f)
        {
            SpawnBullet().GetComponent<BulletScript>().direction = "Left";
            PrepereForNextAction();
        }

        if (Input.GetAxisRaw("Vertical") == 1f)
        {
            SpawnBullet().GetComponent<BulletScript>().direction = "Up";
            PrepereForNextAction();
        }

        if (Input.GetAxisRaw("Vertical") == -1f)
        {
            SpawnBullet().GetComponent<BulletScript>().direction = "Down";
            PrepereForNextAction();
        }
    }

    private GameObject SpawnBullet()
    {
        GameObject bullet = Instantiate(Addressables.LoadAssetAsync<GameObject>("Bullet").WaitForCompletion(), transform.position, Quaternion.identity);
        bullet.GetComponent<BulletScript>().direction = "Right";
        bullet.GetComponent<BulletScript>().strength = valueRolled;
        bullet.GetComponent<BulletScript>().owner = gameObject;
        bulletExists = true;
        return bullet;
    }

    private void MeleeAttack()
    {


        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
        {
            switch (Input.GetAxisRaw("Horizontal"))
            {
                case 1f:
                    SpawnMelee().GetComponent<BulletScript>().direction = "Right";
                    break;
                case -1f:
                    SpawnMelee().GetComponent<BulletScript>().direction = "Left";
                    break;
            }

            PrepereForNextAction();
        }

        if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {

            switch (Input.GetAxisRaw("Vertical"))
            {
                case 1f:
                    SpawnMelee().GetComponent<BulletScript>().direction = "Up";
                    break;
                case -1f:
                    SpawnMelee().GetComponent<BulletScript>().direction = "Down";
                    break;
            }

            PrepereForNextAction();
        }
    }

    private GameObject SpawnMelee()
    {
        int range;

        if (valueRolled >= 5)
        {
            range = 2;
        }
        else
        {
            range = 1;
        }

        GameObject melee = Instantiate(Addressables.LoadAssetAsync<GameObject>("Melee").WaitForCompletion(), transform.position, Quaternion.identity);
        melee.GetComponent<BulletScript>().strength = valueRolled * 2;
        melee.GetComponent<BulletScript>().length = range;
        melee.GetComponent<BulletScript>().owner = gameObject;
        bulletExists = true;
        return melee;
    }

    private void Heal()
    {
        health += (int)Mathf.Floor(valueRolled * 1.5f);
        PrepereForNextAction();
    }
    
    private void Defend()
    {
        shieldDurability = 2;
        damageResistanceStrength = valueRolled;
        PrepereForNextAction();
    }

    private void PrepereForNextAction()
    {
        actionTaken = "None";
        actionsLeft--;
        valueRolled = 0;
        valueIsRolled = false;
    }

    public int RollAValue(int minValue, int maxValue) => Random.Range(minValue, maxValue + 1);

    public void TakeDamage(int amount)
    {
        if (shieldDurability <= 0)
        {
            health -= amount;
        }
        else
        {
            if (amount >= damageResistanceStrength)
            {
                health -= amount;
            }
            else
            {
                shieldDurability--;
            }
        }
    }
}
