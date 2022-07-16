using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float moveSpeed;

    void Awake()
    {
        //transform.LookAt(Input.mousePosition);
    }

    void FixedUpdate()
    {
        transform.position += transform.up * moveSpeed;
    }
}
