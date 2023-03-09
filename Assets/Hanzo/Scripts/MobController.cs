using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hanzo
{
    public class MobController : MonoBehaviour
    {
        public GameObject playerSpawnerGo;
        public MobSpawnerController mobSpawnerController;
        Animator mobAnim;

        // Start is called before the first frame update
        void Start()
        {
            mobAnim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(mobSpawnerController.isMobAttack == true){
                  transform.position = Vector3.MoveTowards(transform.position, playerSpawnerGo.transform.position, Time.fixedDeltaTime * 1.5f);
                mobAnim.SetBool("run", true);
            }
              
        }
    }

}
