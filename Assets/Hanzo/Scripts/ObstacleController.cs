using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hanzo
{
    public class ObstacleController : MonoBehaviour
{
   
    public GameObject playerSpawnerGO;

    private void Start() {
        playerSpawnerGO = GameObject.FindGameObjectWithTag("PlayerSpawner");
        
    }


    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            // Destroy(other.gameObject);
            SpawnController.Instance.CopGotKilled(other.gameObject);
        }
    }
}

}
