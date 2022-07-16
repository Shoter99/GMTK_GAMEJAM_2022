using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] enemyList;
    private int randEnemy, randNumOfEnemies;
    void Start()
    {
        randEnemy = Random.Range(0, enemyList.Length);
        randNumOfEnemies = Random.Range(1, 4);
        for (int i = 0; i < randNumOfEnemies; i++)
        {
            int randXPos = Random.Range(0, 6);
            int randYPos = Random.Range(0, 6);
            Instantiate(enemyList[0], new Vector3(transform.position.x + randXPos, transform.position.y + randYPos, 0), Quaternion.identity);
        }
        
    }


}
