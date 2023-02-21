using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hanzo
{
    public class GatewayController : MonoBehaviour
    {

        public int gateValue;

        public TMPro.TextMeshProUGUI gateText;

        bool isGatePassed = false;
        // SpawnController spawnController;
        public GameObject PlayerPrefabSpawner;



        void Awake()
        {
            PlayerPrefabSpawner = GameObject.Find("PlayerSpawner");
        }

       
        void Start()
        {
            AddGateValue();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isGatePassed = true;
                SpawnController.Instance.SpawnPlayer(3);
                Destroy(this.gameObject);
            }
        }

        void AddGateValue()
        {
            gateText.text = gateValue.ToString();
        }

    }
}

