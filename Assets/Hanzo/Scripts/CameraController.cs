using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hanzo
{
    public class CameraController : MonoBehaviour
    {
        public Transform player;
        public Vector3 offset;
        


        // Start is called before the first frame update
        void Start()
        {
           
        }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = player.position + offset;
        }

       
    }

}


