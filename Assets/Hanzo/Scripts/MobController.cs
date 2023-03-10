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
        public bool isMobAlive;

        // Start is called before the first frame update
        void Start()
        {
            mobAnim = GetComponent<Animator>();
            isMobAlive = true;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (mobSpawnerController.isMobAttack == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerSpawnerGo.transform.position, Time.fixedDeltaTime * 1.5f);
                mobAnim.SetBool("run", true);
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
            {
                mobSpawnerController.MobGotShot(gameObject);
                Destroy(other.gameObject);
            }
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.CompareTag("Player") && isMobAlive == true)
            {
                isMobAlive = false;
                mobSpawnerController.MobAttackCop(col.gameObject, this.gameObject);
                Destroy(this.gameObject);
            }
        }



    }

}
