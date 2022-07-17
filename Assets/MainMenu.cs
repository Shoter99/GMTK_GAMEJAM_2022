using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject main;
    public GameObject settings;
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
    public void ToggleSettings()
    {
        main.active = false;
        settings.active = true;
    }
    public void GoBack()
    {
        main.active = true;
        settings.active = false;
    }
    public void Toggle_Fullscreen(bool isToggled)
    {
        Screen.fullScreen = isToggled;
        print(isToggled);
    }
}
