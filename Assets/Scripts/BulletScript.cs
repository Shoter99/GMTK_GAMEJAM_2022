using UnityEngine;

public sealed class BulletScript : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [HideInInspector]
    public string direction;

    public int strength;

    public int length = 0;

    public GameObject owner;


    private void Start()
    {
        if (length == 0)
        {
            length = strength;
        }
    }

    void FixedUpdate()
    {
        switch (direction)
        {
            case "Right":
                transform.position += transform.right * moveSpeed;
                break;
            case "Left":
                transform.position -= transform.right * moveSpeed;
                break;
            case "Up":
                transform.position += transform.up * moveSpeed;
                break;
            case "Down":
                transform.position -= transform.up * moveSpeed;
                break;
        }
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, owner.transform.position) >= length)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject == owner)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(strength);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemies>().health = collision.gameObject.GetComponent<Enemies>().TakeDamage(collision.gameObject.GetComponent<Enemies>().health, strength);
            Debug.Log("Test");
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (owner.CompareTag("Player"))
        {
            owner.GetComponent<Player>().bulletExists = false;
        }

        if (owner.CompareTag("Enemy"))
        {
            owner.GetComponent<FireEnemy>().bulletExists = false;
        }
    }
}
