using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hanzo
{
    public class SpawnController : MonoBehaviour
    {
        public float speed = 5f;
        public float hSpeed;
        float maxXposition = 4.10f;
        public GameObject PlayerPrefab;


        // Start is called before the first frame update
        void Start()
        {
             SpawnPlayer(3);
        }

        // Update is called once per frame
        void Update()
        {
            float touchX = 0;
            float newXValue = 0;
            
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
            newXValue = Mathf.Clamp(newXValue,-maxXposition, maxXposition);
            Vector3 newPos = new Vector3(newXValue, transform.position.y, transform.position.z + Time.deltaTime * speed);
            transform.position = newPos;
        }

         public void SpawnPlayer(int playerCount)
        {
            //Spawn Players inside Player Child
            for (int i = 0; i < playerCount; i++)
            {
                 Instantiate(PlayerPrefab, PlayerPosition(), Quaternion.identity, transform);
            }
           
        }

        public Vector3 PlayerPosition()
        {
            Vector3 position = Random.insideUnitSphere * 0.1f;
            Vector3 newPos = transform.position + position;
            return newPos;
        }
    }

}
