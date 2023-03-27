using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Hanzo
{
    public class SpawnController : MonoBehaviour
    {
        public static SpawnController Instance;


        public float speed = 5f;
        public float hSpeed;
        float maxXposition = 4.10f;
        public GameObject PlayerPrefab;

        // [SerializeField]
        bool isPlayerMoving = false;

        public List<GameObject> playerList;
        [SerializeField] private TextMeshProUGUI scoreText;

        private int _score;
        internal int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        const string HIGHSCORE = "HIGH_SCORE";
        const string SAVEDLEVEL = "SAVED_LEVEL";

        public AudioClip popSound;
        [SerializeField] GameObject camera;
        public AudioSource source;


        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            // SpawnPlayer(3);
            Score = 0;
            scoreText = GameObject.Find("scoreText").GetComponent<TextMeshProUGUI>();
            scoreText.text = Score.ToString();

            camera = GameObject.Find("Main Camera");
            source = camera.GetComponent<AudioSource>();
            popSound = SoundManager.instance.sounds[1].clip;

        }

        // Update is called once per frame
        void Update()
        {
            float touchX = 0;
            float newXValue = 0;

            if (isPlayerMoving == false)
            {
                return;
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                hSpeed = 250f;
                touchX = Input.GetTouch(0).deltaPosition.x / Screen.width;
            }

            if (Input.GetMouseButton(0))
            {
                hSpeed = 500f;
                touchX = Input.GetAxis("Mouse X");
            }

            newXValue = transform.position.x + hSpeed * touchX * Time.deltaTime;
            newXValue = Mathf.Clamp(newXValue, -maxXposition, maxXposition);
            Vector3 newPos = new Vector3(newXValue, transform.position.y, transform.position.z + Time.deltaTime * speed);
            transform.position = newPos;

            scoreText.text = Score.ToString();

        }

        public void SpawnPlayer(int playerCount, GatewayType gatewayType)
        {
            if (gatewayType == GatewayType.additionType)
            {
                //Spawn Players inside Player Child
                for (int i = 0; i < playerCount; i++)
                {
                    GameObject NewPlayerGO = Instantiate(PlayerPrefab, PlayerPosition(), Quaternion.identity, transform);
                    playerList.Add(NewPlayerGO);


                }
            }
            else if (gatewayType == GatewayType.multiplyType)
            {
                int newPlayerCount = (playerList.Count * playerCount) - playerList.Count;

                for (int i = 0; i < newPlayerCount; i++)
                {
                    GameObject newPlayerGO = Instantiate(PlayerPrefab, PlayerPosition(), Quaternion.identity, transform);
                    playerList.Add(newPlayerGO);
                }

            }
            Score = playerList.Count;
            source.PlayOneShot(popSound);

            // GameObject smoke = Resources.Load<GameObject>("Smoke");
            // Instantiate(smoke, transform.position, Quaternion.identity);
           


        }



        public Vector3 PlayerPosition()
        {
            Vector3 position = Random.insideUnitSphere * 0.1f;
            Vector3 newPos = transform.position + position;
            newPos.y *= 0;
            return newPos;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Finish Line"))
            {
                // isPlayerMoving = false;
                GameManager.instance.ShowWinPanel();
                GameObject.Find("Main Camera").GetComponent<CameraController>().enabled = false;
                StopPlayer();
                int highscore = PlayerPrefs.GetInt(HIGHSCORE) + Score;
                PlayerPrefs.SetInt(HIGHSCORE, highscore);

            }
        }

        public void CopGotKilled(GameObject copGO)
        {
            playerList.Remove(copGO);
            Destroy(copGO);
            DetectCopCount();

        }

        void DetectCopCount()
        {
            Score = playerList.Count;

            if (playerList.Count <= 0)
            {
                StopPlayer();
                GameManager.instance.ShowFailPanel();
            }
        }

        public void MobDetected(GameObject target)
        {
            isPlayerMoving = false;
            LookAtEnemy(target);
            StartShootingAnim();
        }

        private void LookAtEnemy(GameObject target)
        {
            Vector3 dir = target.transform.position - transform.position;
            // Vector3 dir = transform.position - target.transform.position ;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            lookRotation.x = 0;
            lookRotation.z = 0;
            transform.rotation = lookRotation;
        }

        void LookFoward()
        {
            transform.rotation = Quaternion.identity;
        }

        public void AllMobsKilled()
        {
            LookFoward();
            MovePlayer();

        }

        public void MovePlayer()
        {
            isPlayerMoving = true;
            StartRunningAnim();

        }

        void StopPlayer()
        {
            isPlayerMoving = false;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var playerAnim in players)
            {
                playerAnim.GetComponent<Animator>().Play("Idle");
            }
        }

        void StartShootingAnim()
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                PlayerController cop = playerList[i].GetComponent<PlayerController>();
                cop.StartShooting();
            }
        }

        void StartRunningAnim()
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                PlayerController cop = playerList[i].GetComponent<PlayerController>();
                cop.StopShooting();
            }
        }


    }
}


