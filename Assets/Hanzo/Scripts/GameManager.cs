using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hanzo;
public class GameManager : MonoBehaviour
{
 #region  Gameplay manager
 
         [SerializeField]
    GameObject menuPanel, failPanel, winPanel;
    public static GameManager instance;


    // Start is called before the first frame update
   void Awake()
{
    if (instance != null && instance != this)
    {
        Destroy(this.gameObject);
    }
    else
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}


    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        menuPanel.SetActive(false);
        SpawnController.Instance.MovePlayer();
    }

    public void ShowFailPanel()
    {
        failPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
         int levelIndex = SceneManager.GetActiveScene().buildIndex;
                PlayerPrefs.SetInt("SAVED_LEVEL",levelIndex);
    }

    public void LoadNextLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
 #endregion
   
}
