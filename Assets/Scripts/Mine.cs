using UnityEngine;

public class Mine : MonoBehaviour
{
    public int strength;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(strength);
            Destroy(gameObject);
        }
    }
}
