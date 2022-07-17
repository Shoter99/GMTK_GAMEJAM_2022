using UnityEngine;
using UnityEngine.AddressableAssets;

public sealed class MinerEnemy : Enemies
{
    private bool onASquare = false;

    private void Start()
    {
        EnemyManager.Instance.minerEnemies.Add(this);
    }

    private void OnDestroy()
    {
        EnemyManager.Instance.minerEnemies.Remove(this);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, movePoint.position) == 0 &&  !onASquare)
        {
            onASquare = true;
            
            if (Random.Range(1, 6) == 1)
            {
                GameObject mine = Instantiate(Addressables.LoadAssetAsync<GameObject>("Mine").WaitForCompletion(), transform.position, Quaternion.identity);
                mine.GetComponent<Mine>().strength = RollNumber(minValue, maxValue);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, movePoint.position) != 0)
            {
                onASquare = false;
            }
        }
    }
}
