using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    public List<Enemies> enemies = new List<Enemies>();

    private bool enemiesMoving = false;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (GameManager.Instance.turn == "Enemies")
        {
            if (!enemiesMoving)
            {
                foreach (Enemies enemy in enemies)
                {
                    enemy.TakeAction(1, 6, enemy.IsPlayerNear(enemy.UpRaycast, enemy.DownRaycast, enemy.RightRaycast, enemy.LeftRaycast), enemy.moveSpeed, enemy.movePoint);
                }
                enemiesMoving = true;
            }
            else
            {
                int enemiesNotMoving = 0;
                foreach (Enemies enemy in enemies)
                {
                    if (!enemy.isMoving)
                    {
                        enemiesNotMoving++;
                    }
                }
                if (enemiesNotMoving == enemies.Count)
                {
                    enemiesMoving = false;
                    GameManager.Instance.turn = "Player";
                }
            }
        }
    }
}
