using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Enemies : MonoBehaviour
{
    public int health = 10;

    private int moveDistance;

    [SerializeField]
    private int minValue, maxValue;

    [SerializeField]
    private float raycastLength;

    [Range(0, 20)]
    public float moveSpeed;

    public bool isPlayerNear = false, isMoving = false;

    public Transform movePoint;

    public List<Transform> raycasts = new List<Transform>();

    private Player player;

    //public bool actionWaTaken = false;

    private void Start()
    {
        movePoint.parent = null;
        EnemyManager.Instance.enemies.Add(this);
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Sprawdza, czy gracz jest w pobli�u by mog� zada� damage. Plan jest by przerobi� to w strzalanie
    public bool IsPlayerNear(Transform UpRaycast, Transform DownRaycast, Transform RightRaycast, Transform LeftRaycast)
    {
        if (Physics2D.Raycast(UpRaycast.position, Vector3.up, raycastLength))
            return true;

        if (Physics2D.Raycast(DownRaycast.position, Vector3.down, raycastLength))
            return true;

        if (Physics2D.Raycast(RightRaycast.position, Vector3.right, raycastLength))
            return true;

        if (Physics2D.Raycast(LeftRaycast.position, Vector3.left, raycastLength))
            return true;

        return false;
    }

    private void FixedUpdate()
    {
        // Nie potrzebne, tylko do Debugowania
        isPlayerNear = IsPlayerNear(raycasts[0], raycasts[1], raycasts[2], raycasts[3]);
    }

    public void TakeAction(int minValue, int maxValue, bool isPlayerNear, float moveSpeed, Transform movePoint)
    {
        //Losuje pomiedzy atakiem i poruszanie sie
        switch (Random.Range(0, 2))
        {
            case 0:
                if (isPlayerNear)
                {
                    Attack();
                }
                break;
            case 1:
                moveDistance = Random.Range(minValue, maxValue);
                isMoving = true;
                StartCoroutine(Move(moveSpeed, movePoint));
                break;
        }
    }

    // Raczej glowna roznica pomiedzy przeciwnikami bedzie rodzaj ataku. Pomysl jest by przeciwnicy dziedziczyli ta klasa
    // i uzywali public override Attack() {}, oraz zmieniali zmienne w inspectorze by rzuczali innymi koscmi. Przyklad w BasicEnemy
    public virtual void Attack()
    {
        //Instantiate(Addressables.LoadAssetAsync<GameObject>("Bullet").WaitForCompletion(), transform.position, Quaternion.identity);
        player.health = player.TakeDamage(player.health, 1);
    }

    public IEnumerator Move(float moveSpeed, Transform movePoint)
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) == 0)
        {
            // Porusza sie dopoki nie wyczerpie mu sie wylosowana odleglosc
            if (moveDistance == 0)
            {
                isMoving = false;
                yield break;
            }

            //Zmienne do zmienienia, gdy bedzie znana odlegloc pomiedzy kwadratami

            switch (Random.Range(1, 4))
            {
                case 1:
                    movePoint.transform.position += new Vector3(1, 0, 0);
                    break;
                case 2:
                    movePoint.transform.position += new Vector3(-1, 0, 0);
                    break;
                case 3:
                    movePoint.transform.position += new Vector3(0, 1, 0);
                    break;
                case 4:
                    movePoint.transform.position += new Vector3(0, -1, 0);
                    break;
            }

            moveDistance--;
        }

        yield return new WaitForEndOfFrame();
        StartCoroutine(Move(moveSpeed, movePoint));
    }

    private void OnDestroy()
    {
        EnemyManager.Instance.enemies.Remove(this);
    }
}
