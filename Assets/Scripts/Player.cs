using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public sealed class Player : MonoBehaviour
{      
    public static Player Instance { get; private set; }

    private struct Values
    {
        public int moveValue;
        public int fireValue;
        public int meleeValue;
        public int healValue;
        public int defendValue;

        public void RollNumbers(int minValue, int maxValue)
        {
            moveValue = RollAValue(minValue, maxValue);
            fireValue = RollAValue(minValue, maxValue);
            meleeValue = RollAValue(minValue, maxValue);
            healValue = RollAValue(minValue, maxValue);
            defendValue = RollAValue(minValue, maxValue);
        }

        private int RollAValue(int minValue, int maxValue) => Random.Range(minValue, maxValue + 1);
    }

    Values values;

    [Range(0, 20)]
    public int health = 15;

    [SerializeField]
    private float moveSpeed = 1f;

    public int maxActions;

    public int minValue, maxValue;

    public string actionTaken = "None";

    public GameObject deathScreen;
    public Sprite[] diceSprites;
    public Image[] dices;

    [HideInInspector]
    public int actionsLeft, damageResistanceStrength = 0, shieldDurabality = 0;

    [HideInInspector]
    public bool valueIsRolled = false, bulletExists = false;

    private readonly List<Transform> raycasts = new List<Transform>();

    private Transform movePoint;

    //public GameObject bulletPrefab;

    //public GameObject meleePrefab;



    private void Awake()
    {
        Instance = this;
        actionsLeft = maxActions;
        movePoint = transform.GetChild(0).transform;
        movePoint.parent = null;

        for (int i = 0; i < 4; i++)
        {
            raycasts.Add(transform.GetChild(i).transform);
        }      
    }

    private void LateUpdate()
    {
        if (health <= 0)
        {
            deathScreen.SetActive(true);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.turn == "Enemies" || bulletExists)
            return;

        if (!valueIsRolled)
        {
            valueIsRolled = true;
            values.RollNumbers(minValue, maxValue);
            
            //int[,] diceValues = { { values.meleeValue, 0, 0 }, { values.fireValue, 0, 0 }, { values.moveValue, 0, 0 }, { values.healValue, 0, 0 }, { values.defendValue, 0, 0 } };

            //UiManager.Instance.reroll(diceValues);

            ChangeDiceSprite(diceSprites, dices[2], values.moveValue);
            ChangeDiceSprite(diceSprites, dices[3], values.fireValue);
            ChangeDiceSprite(diceSprites, dices[4], values.meleeValue);
            ChangeDiceSprite(diceSprites, dices[1], values.healValue);
            ChangeDiceSprite(diceSprites, dices[0], values.defendValue);      
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
            if (values.moveValue == 0)
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
                                values.moveValue--;
                                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                            }
                        }
                        else
                        {
                            values.moveValue--;
                            movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                        }
                        break;

                    case -1f:
                        hit = Physics2D.Raycast(raycasts[3].position, Vector2.left, 0.5f);
                        if (hit)
                        {
                            if (!hit.collider.CompareTag("Enemy") && !hit.collider.CompareTag("Walls"))
                            {
                                values.moveValue--;
                                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                            }
                        }
                        else
                        {
                            values.moveValue--;
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
                                values.moveValue--;
                                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                            }
                        }
                        else
                        {
                            values.moveValue--;
                            movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                        }
                        break;

                    case -1f:
                        hit = Physics2D.Raycast(raycasts[1].position, Vector2.down, 0.5f);
                        if (hit)
                        {
                            if (!hit.collider.CompareTag("Enemy") && !hit.collider.CompareTag("Walls"))
                            {
                                values.moveValue--;
                                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                            }
                        }
                        else
                        {
                            values.moveValue--;
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
            bulletExists = true;
            PrepereForNextAction();
        }

        if (Input.GetAxisRaw("Horizontal") == -1f)
        {
            SpawnBullet().GetComponent<BulletScript>().direction = "Left";
            bulletExists = true;
            PrepereForNextAction();
        }

        if (Input.GetAxisRaw("Vertical") == 1f)
        {
            SpawnBullet().GetComponent<BulletScript>().direction = "Up";
            bulletExists = true;
            PrepereForNextAction();
        }

        if (Input.GetAxisRaw("Vertical") == -1f)
        {
            SpawnBullet().GetComponent<BulletScript>().direction = "Down";
            bulletExists = true;
            PrepereForNextAction();
        }
    }

    private GameObject SpawnBullet ()
    {
        //GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        GameObject bullet = Instantiate(Addressables.LoadAssetAsync<GameObject>("Bullet").WaitForCompletion(), transform.position, Quaternion.identity);
        bullet.GetComponent<BulletScript>().strength = values.fireValue;
        bullet.GetComponent<BulletScript>().owner = gameObject;
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

            bulletExists = true;
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

            bulletExists = true;
            PrepereForNextAction();
        }

    }

    private GameObject SpawnMelee()
    {
        int range;

        if (values.meleeValue >= 5)
        {
            range = 2;
        }
        else
        {
            range = 1;
        }

        //GameObject melee = Instantiate(meleePrefab, transform.position, Quaternion.identity);
        GameObject melee = Instantiate(Addressables.LoadAssetAsync<GameObject>("Melee").WaitForCompletion(), transform.position, Quaternion.identity);
        melee.GetComponent<BulletScript>().strength = values.meleeValue * 2;
        melee.GetComponent<BulletScript>().length = range;
        melee.GetComponent<BulletScript>().owner = gameObject;
        return melee;
    }

    private void Heal()
    {
        health += values.healValue;
        UiManager.Instance.updateHealth(health);
        PrepereForNextAction();
    }
    
    private void Defend()
    {
        shieldDurabality = 2;
        damageResistanceStrength = values.defendValue;
        PrepereForNextAction();
    }

    private void PrepereForNextAction()
    {
        values = default;
        valueIsRolled = false;
        actionTaken = "None";
        actionsLeft--;
    }

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
