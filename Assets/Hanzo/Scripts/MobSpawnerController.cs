using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hanzo
{
    public class MobSpawnerController : MonoBehaviour
    {
        public GameObject mob;
        public List<GameObject> mobs = new List<GameObject>();

        
        GameObject playerSpawnerGo;

        public bool isMobAttack = false;


        private void Awake()
        {
            isMobAttack = false;
            playerSpawnerGo = GameObject.FindGameObjectWithTag("PlayerSpawner");
        
        }

        // Start is called before the first frame update
        void Start()
        {
            SpawnMob(3);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SpawnMob(int mobCount)
        {
            for (int i = 0; i < mobCount; i++)
            {
                Quaternion mobRot = Quaternion.Euler(new Vector3(0, 180, 0));
                GameObject mobGO = Instantiate(mob, GetMobPosition(), mobRot, transform);
                MobController mobScript = mobGO.GetComponent<MobController>();
                mobScript.playerSpawnerGo = playerSpawnerGo;
                mobScript.mobSpawnerController = this;
                mobs.Add(mobGO);
            }

        }

        public Vector3 GetMobPosition()
        {
            Vector3 pos = Random.insideUnitSphere * 0.1f;
            Vector3 newPos = transform.position + pos;
            return newPos;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GetComponent<BoxCollider>().enabled = false;
                SpawnController.Instance.MobDetected(gameObject);
                LookAtPlayer(other.gameObject);
                isMobAttack = true;
            }
        }

        private void LookAtPlayer(GameObject target)
        {
            Vector3 dir = transform.position - target.transform.position;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            lookRot.x = 0;
            lookRot.z = 0;

            transform.rotation = lookRot;
        }

        public void MobAttackCop(GameObject cop, GameObject mobGO)
        {
            mobs.Remove(mobGO);
            CheckMobCount();
           SpawnController.Instance.CopGotKilled(cop);
        }

        public void MobGotShot(GameObject theMob)
        {
                mobs.Remove(theMob);
                Destroy(theMob);
                CheckMobCount();
        }

        void CheckMobCount()
        {
            if (mobs.Count <= 0)
               SpawnController.Instance.AllMobsKilled();
        }


    }

}
