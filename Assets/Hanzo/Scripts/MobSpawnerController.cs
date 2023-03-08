using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hanzo
{
    public class MobSpawnerController : MonoBehaviour
    {
        public GameObject mob;
        public List<GameObject> mobs = new List<GameObject>();
        // Start is called before the first frame update
        void Start()
        {
            SpawnMob(3);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SpawnMob(int moobCount)
        {
            GameObject mobGO = Instantiate(mob, GetMobPosition(), Quaternion.identity);
            mobs.Add(mobGO);
        }

        public Vector3 GetMobPosition()
        {
            Vector3 pos = Random.insideUnitSphere * 0.1f;
            Vector3 newPos = transform.position + pos;
            return newPos; 
        }


    }

}
