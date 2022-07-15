using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyManager : MonoBehaviour
{
    public EnemyManager Instance { get; private set; }

    private List<GameObject> enemies = new List<GameObject>();


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (GameManager.Instance.turn == "Enemies")
        {
            foreach (GameObject enemy in enemies)
            {
                //enemy.GetComponent<BasicEnemy>().TakeAction(1, 6, false, 0.2, );
            }
            GameManager.Instance.turn = "Player";
        }
    }
}
