using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hanzo
{
    public class ObstacleController : MonoBehaviour
{
    SpawnController spawnController;
    public GameObject playerSpawnerGO;

    private void Start() {
        playerSpawnerGO = GameObject.FindGameObjectWithTag("PlayerSpawner");
        spawnController = playerSpawnerGO.GetComponent<SpawnController>();
    }


    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            // Destroy(other.gameObject);
            spawnController.CopGotKilled(other.gameObject);
        }
    }
}

}
