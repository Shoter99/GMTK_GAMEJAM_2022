using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    public List<MeleeEnemy> meleeEnemies = new List<MeleeEnemy>();
    public List<FireEnemy> rangeEnemies = new List<FireEnemy>();
    public List<MinerEnemy> minerEnemies = new List<MinerEnemy>();

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
        {
            phase = "Fire";

            foreach (MeleeEnemy enemy in meleeEnemies)
            {
                enemy.rolledValue = enemy.RollNumber(enemy.minValue, enemy.maxValue);
                enemy.storedValue = enemy.rolledValue;
            }

            foreach (FireEnemy enemy in rangeEnemies)
            {
                enemy.rolledValue = enemy.RollNumber(enemy.minValue, enemy.maxValue);
            }

            foreach (MinerEnemy enemy in minerEnemies)
            {
                enemy.rolledValue = enemy.RollNumber(enemy.minValue, enemy.maxValue);
            }
        }

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

                    foreach (MinerEnemy enemy in minerEnemies)
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

                    foreach (MinerEnemy enemy in minerEnemies)
                    {
                        enemy.isMoving = true;
                        StartCoroutine(enemy.Move());
                    }
                }
                break;
            case "Melee":
                if (phaseInProgress)
                {
                    foreach (MeleeEnemy enemy in meleeEnemies)
                    {
                        if (enemy.bulletExists)
                            return;
                    }

                    phaseInProgress = false;

                    phase = "None";

                    foreach (FireEnemy enemy in rangeEnemies)
                    {
                        enemy.bulletFiredThisTurn = false;
                    }

                    GameManager.Instance.turn = "Player";
                    Player.Instance.actionsLeft = 1;

                    break;

                }
                else
                {
                    phaseInProgress = true;

                    foreach (MeleeEnemy enemy in meleeEnemies)
                    {
                        enemy.Attack();
                    }
                }

                break;
        }
    }
}
