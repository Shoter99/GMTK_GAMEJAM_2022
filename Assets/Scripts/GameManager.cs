using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    /*[System.Serializable]
    public struct Squere
    {
        public Vector3 Position;
        public GameObject CurrentObject;

        public Squere(Vector3 position, GameObject currentObject)
        {
            Position = position;
            CurrentObject = currentObject;
        }
    }*/


    public static GameManager Instance { get; private set; }

    public string turn;



    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
