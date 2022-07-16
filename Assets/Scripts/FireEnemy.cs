using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class FireEnemy : Enemies
{
    public override void Attack()
    {
        base.Attack();
        GameObject bullet = Instantiate(Addressables.LoadAssetAsync<GameObject>("Bullet").WaitForCompletion(), transform.position, Quaternion.identity);

        switch (Random.Range(1, 5))
        {
            case 1:
                bullet.GetComponent<BulletScript>().direction = "Up";
                break;
            case 2:
                bullet.GetComponent<BulletScript>().direction = "Down";
                break;
            case 3:
                bullet.GetComponent<BulletScript>().direction = "Right";
                break;
            case 4:
                bullet.GetComponent<BulletScript>().direction = "Left";
                break;
        }

        bullet.GetComponent<BulletScript>().strength = RollNumber(minValue, maxValue);
        bullet.GetComponent<BulletScript>().owner = gameObject;
    }
}
