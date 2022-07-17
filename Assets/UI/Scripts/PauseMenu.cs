using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuView;

    private void Awake()
    {
        pauseMenuView.SetActive(false);
    }
    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenuView.SetActive(true);
        isPaused = true;
    }
    public void UnPause()
    {
        Time.timeScale = 1;
        pauseMenuView.SetActive(false);
        isPaused = false;
    }
    public void GoToMainMenu()
    {
        UnPause();
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print(isPaused);
            if (isPaused)
            {
                UnPause();
            }
            else
            {
                Pause();
            }
        }
    }
}
