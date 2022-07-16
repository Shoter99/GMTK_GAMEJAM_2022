using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [HideInInspector]
    public string direction;

    [HideInInspector]
    public int strength;

    void Awake()
    {
        //transform.LookAt(Input.mousePosition);
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
}
