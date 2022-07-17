using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public GameObject sprite;
    public Sprite[] campfiresSprites;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sprite.GetComponent<SpriteRenderer>().sprite = campfiresSprites[1];
        }
    }
}
