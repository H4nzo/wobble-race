using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hanzo
{
    public enum GatewayType{multiplyType, additionType }
    public class GatewayController : MonoBehaviour
    {
        

        public int gateValue;

        public TMPro.TextMeshProUGUI gateText;


        public GatewayType gatewayType;


        bool isGatePassed = false;

        GateContainerController gateContainerController;


        public GameObject PlayerPrefabSpawner;



        void Awake()
        {
            PlayerPrefabSpawner = GameObject.Find("PlayerSpawner");
            gateContainerController = transform.parent.gameObject.GetComponent<GateContainerController>();
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
                SpawnController.Instance.SpawnPlayer(gateValue, gatewayType);
                gateContainerController.CloseGate();
                Destroy(this.gameObject);
            }
        }

        void AddGateValue()
        {
            switch (gatewayType)
            {
                case GatewayType.multiplyType:
                    gateText.text = "x" +gateValue.ToString();
                    break;
                
                case GatewayType.additionType:
                    gateText.text = "+" +gateValue.ToString();
                    break;
                
                default:
                    break;
            }


        }

    }
}

