using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Hanzo.GameController
{
    public class OverallGameManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI highScoreText;

        private const string SAVED_LEVEL = "SAVED_LEVEL";
        private const string HIGH_SCORE = "HIGH_SCORE";

        public static OverallGameManager instance;


        // Start is called before the first frame update
        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            int highscore = PlayerPrefs.GetInt(HIGH_SCORE);
            highScoreText = GameObject.Find("Text_highScore").GetComponent<TextMeshProUGUI>();
            highScoreText.text = highscore.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            

        }

        public void Play()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt(SAVED_LEVEL, 1);
        }

        public void Continue()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + PlayerPrefs.GetInt(SAVED_LEVEL));
        }

        public void Leaderboard()
        {
            //show leaderboard score from playFab SDK
        }

        public void Quit()
        {
            Application.Quit();
        }

    }
}

