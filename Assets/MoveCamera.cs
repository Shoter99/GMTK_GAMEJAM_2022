using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        float posX = transform.position.x - player.transform.position.x;
        float posY = transform.position.y - player.transform.position.y;
        print(posX);
        if(posX >= 5)
        {
            transform.position = new Vector3(transform.position.x - 10, transform.position.y, -10);
        }
        if(posX <= -5)
        {
            transform.position = new Vector3(transform.position.x + 10, transform.position.y, -10);
        }
        if(posY >= 5)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 10, -10);
        }
        if(posY <= -5)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 10, -10);
        }
    }
}
