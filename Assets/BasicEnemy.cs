using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public int health= 10;

    public int minValue = 1;
    public int maxValue = 2;

    public bool isPlayerNear = false;

    public Transform UpRaycast, DownRaycast, RightRaycast, LeftRaycast;
}
