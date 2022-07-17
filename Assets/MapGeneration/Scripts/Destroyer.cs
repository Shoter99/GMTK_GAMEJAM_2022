using UnityEngine;

public class Destroyer : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player") && !other.CompareTag("Enemy") && !other.CompareTag("Respawn") && !other.CompareTag("Bullet"))
        Destroy(other.gameObject);

    }
}
