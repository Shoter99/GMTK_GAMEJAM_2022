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
        main.SetActive(false);
        settings.SetActive(true);
    }
    public void GoBack()
    {
        main.SetActive(true);
        settings.SetActive(false);
    }
    public void Toggle_Fullscreen(bool isToggled)
    {
        Screen.fullScreen = isToggled;
        print(isToggled);
    }
    public void Toggle_Music(bool isToggled)
    {
        AudioSource am = FindObjectOfType<AudioManager>().GetComponent<AudioSource>();
        am.mute = !isToggled;
     
    }
}
