using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject pauseButton, pauseUI;

    public static bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (isPaused == true)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        pauseUI.SetActive(true);
    }

    public void Resume()
    {
        isPaused = !true;
        Time.timeScale = 1f;
        pauseButton.SetActive(!false);
        pauseUI.SetActive(!true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Resume();
    }


    public void Quit()
    {
         SceneManager.LoadScene(0);
        
    }








}
