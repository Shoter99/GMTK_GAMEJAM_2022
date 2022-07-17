using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void Start_Game()
    {
        SceneManager.LoadScene("TestScene");
        print("GameStarted");
    }
    public void Exit_Game()
    {
        Application.Quit();
        print("Exited");
    }
}
