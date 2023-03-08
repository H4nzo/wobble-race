using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hanzo
{
    public class ObstacleController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            Destroy(other.gameObject);
        }
    }
}

}
