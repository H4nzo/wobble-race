using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hanzo
{
    public class SpawnController : MonoBehaviour
    {
        public static SpawnController Instance;


        public float speed = 5f;
        public float hSpeed;
        float maxXposition = 4.10f;
        public GameObject PlayerPrefab;

        [SerializeField]
        bool isPlayerMoving = true;

        public List<GameObject> playerList;

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
        }



        public Vector3 PlayerPosition()
        {
            Vector3 position = Random.insideUnitSphere * 0.1f;
            Vector3 newPos = transform.position + position;
            newPos.y *= 0f;
            return newPos;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Finish Line"))
            {
                isPlayerMoving = false;
            }
        }

        public void CopGotKilled(GameObject copGO)
        {
                playerList.Remove(copGO);
                Destroy(copGO);
        }
    }
}


