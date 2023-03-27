using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyObject : MonoBehaviour
{
    public float timeTaken = .1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,timeTaken);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
