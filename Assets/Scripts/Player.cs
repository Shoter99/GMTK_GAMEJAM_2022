using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public sealed class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public int health = 10;
    public int actionsLeft = 2;
    public int damageResistanceStrength = 0;
    public int shieldDurabality = 0;

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
                valueIsRolled = false;
                actionsLeft--;
                actionTaken = "None";
                return;
            }

            RaycastHit2D hit;

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                switch (Input.GetAxisRaw("Horizontal"))
                {
                    case 1f:
                        hit = Physics2D.Raycast(raycasts[2].position, Vector2.right, 1);
                        if (!hit)
                        {
                            valueRolled--;
                            movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                        }
                        break;

                    case -1f:
                        hit = Physics2D.Raycast(raycasts[3].position, Vector2.left, 1);
                        if (!hit)
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
                        hit = Physics2D.Raycast(raycasts[0].position, Vector2.up, 1);
                        if (!hit)
                        {
                            valueRolled--;
                            movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                        }
                        break;

                    case -1f:
                        hit = Physics2D.Raycast(raycasts[1].position, Vector2.down, 1);
                        if (!hit)
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
            GameObject bullet = Instantiate(Addressables.LoadAssetAsync<GameObject>("Bullet").WaitForCompletion(), transform.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().direction = "Right";
            bullet.GetComponent<BulletScript>().strength = valueRolled;
            bullet.GetComponent<BulletScript>().owner = gameObject;
            actionTaken = "None";
            valueRolled = 0;
            valueIsRolled = false;
            bulletExists = true;
            actionsLeft--;
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
            actionsLeft--;
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
            actionsLeft--;
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
            actionsLeft--;
        }
    }

    private void MeleeAttack()
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

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
        {
            GameObject melee = Instantiate(Addressables.LoadAssetAsync<GameObject>("Melee").WaitForCompletion(), transform.position, Quaternion.identity);
            melee.GetComponent<BulletScript>().strength = valueRolled*2;
            melee.GetComponent<BulletScript>().length = range;
            melee.GetComponent<BulletScript>().owner = gameObject;

            switch (Input.GetAxisRaw("Horizontal"))
            {
                case 1f:
                    melee.GetComponent<BulletScript>().direction = "Right";
                    break;
                case -1f:
                    melee.GetComponent<BulletScript>().direction = "Left";
                    break;
            }

            bulletExists = true;
            valueRolled = 0;
            valueIsRolled = false;
            actionTaken = "None";
            actionsLeft--;
        }

        if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            GameObject melee = Instantiate(Addressables.LoadAssetAsync<GameObject>("Melee").WaitForCompletion(), transform.position, Quaternion.identity);
            melee.GetComponent<BulletScript>().strength = valueRolled*2;
            melee.GetComponent<BulletScript>().length = range;
            melee.GetComponent<BulletScript>().owner = gameObject;

            switch (Input.GetAxisRaw("Vertical"))
            {
                case 1f:
                    melee.GetComponent<BulletScript>().direction = "Up";
                    break;
                case -1f:
                    melee.GetComponent<BulletScript>().direction = "Down";
                    break;
            }

            bulletExists = true;
            valueRolled = 0;
            valueIsRolled = false;
            actionTaken = "None";
            actionsLeft--;
        }

    }

    private void Heal()
    {
        health += (int)Mathf.Floor(valueRolled * 1.5f);
        valueRolled = 0;
        valueIsRolled = false;
        actionTaken = "None";
        actionsLeft--;
    }
    
    private void Defend()
    {
        shieldDurabality = 2;
        damageResistanceStrength = valueRolled;
        actionTaken = "None";
        actionsLeft--;
        valueRolled = 0;
        valueIsRolled = false;
    }

    public int RollAValue(int minValue, int maxValue) => Random.Range(minValue, maxValue + 1);

    public void TakeDamage(int amount)
    {
        if (shieldDurabality == 0)
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
                shieldDurabality--;
            }
        }
    }
}
