using UnityEngine;

public sealed class BulletScript : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [HideInInspector]
    public string direction;

    public int strength;

    public GameObject owner;


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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject == owner)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().health = collision.gameObject.GetComponent<Player>().TakeDamage(collision.gameObject.GetComponent<Player>().health, strength);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemies>().health = collision.gameObject.GetComponent<Enemies>().TakeDamage(collision.gameObject.GetComponent<Enemies>().health, strength);
            Destroy(gameObject);
        }
    }
}
