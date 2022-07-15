using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyManager : MonoBehaviour
{
    public EnemyManager Instance { get; private set; }

    private List<Enemies> enemies = new List<Enemies>();


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (GameManager.Instance.turn == "Enemies")
        {
            foreach (Enemies enemy in enemies)
            {
                enemy.TakeAction(1, 6, enemy.IsPlayerNear(enemy.UpRaycast, enemy.DownRaycast, enemy.RightRaycast, enemy.LeftRaycast), enemy.moveSpeed, enemy.movePoint);
            }
            GameManager.Instance.turn = "Player";
        }
    }
}
