using System.Collections.Generic;
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
        // Kiedy jest tura przeciwnikow, Enemy Manager na razie wywo³e funkje TakeAction() wszystkim przeciwnikom rownoczesnie
        if (GameManager.Instance.turn == "Enemies")
        {
            if (!enemiesMoving)
            {
                foreach (Enemies enemy in enemies)
                {
                    enemy.TakeAction(enemy.minValue, enemy.maxValue, enemy.IsPlayerNear(enemy.raycasts[0], enemy.raycasts[1], enemy.raycasts[2], enemy.raycasts[3]), enemy.moveSpeed, enemy.movePoint);
                }
                enemiesMoving = true;
            }
            else
            {
                // Kiedy wszyscy przeciwnicy wykonali swoj¹ akcje, zaczyna siê tura gracza
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
