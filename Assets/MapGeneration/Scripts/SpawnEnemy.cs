using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] enemyList;
    private int randEnemy, randNumOfEnemies;
    void Start()
    {
        randNumOfEnemies = Random.Range(1, 4);
        for (int i = 0; i < randNumOfEnemies; i++)
        {
            randEnemy = Random.Range(0, enemyList.Length);
            int randXPos = Random.Range(1, 5);
            int randYPos = Random.Range(1, 5);
            Instantiate(enemyList[randEnemy], new Vector3(transform.position.x + .5f + randXPos, transform.position.y + .5f + randYPos, 0), Quaternion.identity);
        }
        
    }


}
