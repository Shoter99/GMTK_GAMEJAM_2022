using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    public List<MeleeEnemy> meleeEnemies = new List<MeleeEnemy>();
    public List<FireEnemy> rangeEnemies = new List<FireEnemy>();
    public List<Enemies> sapperEnemies = new List<Enemies>();

    public string phase = "None";

    private bool phaseInProgress = false;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (GameManager.Instance.turn == "Player")
            return;


        if (phase == "None")
            phase = "Fire";

        switch (phase)
        {
            case "Fire":
                if (phaseInProgress)
                {
                    foreach (FireEnemy enemy in rangeEnemies)
                    {
                        if (enemy.bulletExists)
                            return;
                    }

                    phaseInProgress = false;
                    phase = "Move";
                }
                else
                {
                    phaseInProgress = true;
                    foreach (FireEnemy enemy in rangeEnemies)
                    {
                        enemy.Attack();
                    }
                }
                break;
            case "Move":
                if (phaseInProgress)
                {
                    foreach (MeleeEnemy enemy in meleeEnemies)
                    {
                        if (enemy.isMoving)
                            return;
                    }

                    foreach (FireEnemy enemy in rangeEnemies)
                    {
                        if (enemy.isMoving)
                            return;
                    }

                    foreach (Enemies enemy in sapperEnemies)
                    {
                        if (enemy.isMoving)
                            return;
                    }

                    phaseInProgress = false;
                    phase = "Melee";
                }
                else
                {
                    phaseInProgress = true;

                    foreach (MeleeEnemy enemy in meleeEnemies)
                    {
                        enemy.isMoving = true;
                        StartCoroutine(enemy.Move());
                    }

                    foreach (FireEnemy enemy in rangeEnemies)
                    {
                        if (!enemy.bulletFiredThisTurn)
                        {
                            enemy.isMoving = true;
                            StartCoroutine(enemy.Move());
                        }
                    }

                    foreach (Enemies enemy in sapperEnemies)
                    {
                        enemy.isMoving = true;
                        StartCoroutine(enemy.Move());
                    }
                }
                break;
            case "Melee":
                foreach (MeleeEnemy enemy in meleeEnemies)
                {
                    enemy.Attack();
                }

                phase = "None";

                foreach (FireEnemy enemy in rangeEnemies)
                {
                    enemy.bulletFiredThisTurn = false;
                }

                GameManager.Instance.turn = "Player";
                Player.Instance.actionsLeft = 2;

                break;
        }
    }
}
