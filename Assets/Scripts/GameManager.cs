using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string turn;


    private void Awake()
    {
        Instance = this;
    }
}
