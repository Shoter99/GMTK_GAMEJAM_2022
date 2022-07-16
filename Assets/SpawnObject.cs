using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] objectsList;
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, objectsList.Length);
        Instantiate(objectsList[rand], transform.position, Quaternion.identity);
    }


}
