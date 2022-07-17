using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public sealed class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public int health = 10;
    public int actionsLeft = 2;
    public int damageResistanceStrength = 0;
    public int shieldDurabality = 0;

    public bool valueIsRolled = false, bulletExists = false;


    public Sprite[] diceSprites;
    public Image[] dices;

    [SerializeField]
    [Range(0, 20)]
    private float moveSpeed = 1f;

    public int minValue, maxValue;

    public int moveValue, fireValue, meleeValue, healValue, defendValue;

    public Dictionary<string, int> values = new Dictionary<string, int>();

    public string actionTaken = "None";

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
            moveValue = RollAValue(minValue, maxValue);
            fireValue = RollAValue(minValue, maxValue);
            meleeValue = RollAValue(minValue, maxValue);
            healValue = RollAValue(minValue, maxValue);
            defendValue = RollAValue(minValue, maxValue);
            
            int[,] diceValues = { { meleeValue, 0, 0 }, { fireValue, 0, 0 }, { moveValue, 0, 0 }, { healValue, 0, 0 }, { defendValue, 0, 0 } };

            //UiManager.Instance.reroll(diceValues);

            ChangeDiceSprite(diceSprites, dices[2], moveValue);
            ChangeDiceSprite(diceSprites, dices[3], fireValue);
            ChangeDiceSprite(diceSprites, dices[4], meleeValue);
            ChangeDiceSprite(diceSprites, dices[1], healValue);
            ChangeDiceSprite(diceSprites, dices[0], defendValue);
            
            
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
    private void ChangeDiceSprite(Sprite[] sprite, Image img, int num)
    {
        print(num);
        print(sprite);
        print(img);
        switch(num){
            case 1:
                img.sprite = sprite[0];
                break;
            case 2:
                img.sprite = sprite[1];
                break;
            case 3:
                img.sprite = sprite[2];
                break;
            case 4:
                img.sprite = sprite[3];
                break;
            case 5:
                img.sprite = sprite[4];
                break;
            case 6:
                img.sprite = sprite[5];
                break;
                

        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) == 0f)
        {
            if (moveValue == 0)
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
                        hit = Physics2D.Raycast(raycasts[2].position, Vector2.right, 0.5f);

                        if (hit)
                        {
                            if (!hit.collider.CompareTag("Enemy") && !hit.collider.CompareTag("Walls"))
                            {
                                moveValue--;
                                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                            }
                        }
                        else
                        {
                            moveValue--;
                            movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                        }
                        break;

                    case -1f:
                        hit = Physics2D.Raycast(raycasts[3].position, Vector2.left, 0.5f);
                        if (hit)
                        {
                            if (!hit.collider.CompareTag("Enemy") && !hit.collider.CompareTag("Walls"))
                            {
                                moveValue--;
                                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                            }
                        }
                        else
                        {
                            moveValue--;
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
                                moveValue--;
                                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                            }
                        }
                        else
                        {
                            moveValue--;
                            movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                        }
                        break;

                    case -1f:
                        hit = Physics2D.Raycast(raycasts[1].position, Vector2.down, 0.5f);
                        if (hit)
                        {
                            if (!hit.collider.CompareTag("Enemy") && !hit.collider.CompareTag("Walls"))
                            {
                                moveValue--;
                                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                            }
                        }
                        else
                        {
                            moveValue--;
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
            bullet.GetComponent<BulletScript>().strength = fireValue;
            bullet.GetComponent<BulletScript>().owner = gameObject;
            actionTaken = "None";
            fireValue = 0;
            valueIsRolled = false;
            bulletExists = true;
            actionsLeft--;
        }

        if (Input.GetAxisRaw("Horizontal") == -1f)
        {
            GameObject bullet = Instantiate(Addressables.LoadAssetAsync<GameObject>("Bullet").WaitForCompletion(), transform.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().direction = "Left";
            bullet.GetComponent<BulletScript>().strength = fireValue;
            bullet.GetComponent<BulletScript>().owner = gameObject;
            actionTaken = "None";
            fireValue = 0;
            valueIsRolled = false;
            bulletExists = true;
            actionsLeft--;
        }

        if (Input.GetAxisRaw("Vertical") == 1f)
        {
            GameObject bullet = Instantiate(Addressables.LoadAssetAsync<GameObject>("Bullet").WaitForCompletion(), transform.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().direction = "Up";
            bullet.GetComponent<BulletScript>().strength = fireValue;
            bullet.GetComponent<BulletScript>().owner = gameObject;
            actionTaken = "None";
            fireValue = 0;
            valueIsRolled = false;
            bulletExists = true;
            actionsLeft--;
        }

        if (Input.GetAxisRaw("Vertical") == -1f)
        {
            GameObject bullet = Instantiate(Addressables.LoadAssetAsync<GameObject>("Bullet").WaitForCompletion(), transform.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().direction = "Down";
            bullet.GetComponent<BulletScript>().strength = fireValue;
            bullet.GetComponent<BulletScript>().owner = gameObject;
            actionTaken = "None";
            fireValue = 0;
            valueIsRolled = false;
            bulletExists = true;
            actionsLeft--;
        }
    }

    private void MeleeAttack()
    {
        int range;

        if (meleeValue >= 5)
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
            melee.GetComponent<BulletScript>().strength = meleeValue * 2;
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
            meleeValue = 0;
            valueIsRolled = false;
            actionTaken = "None";
            actionsLeft--;
        }

        if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            GameObject melee = Instantiate(Addressables.LoadAssetAsync<GameObject>("Melee").WaitForCompletion(), transform.position, Quaternion.identity);
            melee.GetComponent<BulletScript>().strength = meleeValue * 2;
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
            meleeValue = 0;
            valueIsRolled = false;
            actionTaken = "None";
            actionsLeft--;
        }

    }

    private void Heal()
    {
        health += healValue;
        UiManager.Instance.updateHealth(health);
        healValue = 0;
        valueIsRolled = false;
        actionTaken = "None";
        actionsLeft--;
    }
    
    private void Defend()
    {
        shieldDurabality = 2;
        damageResistanceStrength = defendValue;
        actionTaken = "None";
        actionsLeft--;
        defendValue = 0;
        valueIsRolled = false;
    }

    public int RollAValue(int minValue, int maxValue) => Random.Range(minValue, maxValue + 1);

    public void TakeDamage(int amount)
    {
        if (shieldDurabality <= 0)
        {
            health -= amount;
            UiManager.Instance.updateHealth(health);
        }
        else
        {
            if (amount >= damageResistanceStrength)
            {
                health -= amount;
                UiManager.Instance.updateHealth(health);
            }
            else
            {
                shieldDurabality--;
            }
        }
    }
}
