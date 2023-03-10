using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float destoryTime = 2.7f;


    // Destroys Bullet GameObject in Start Function After the given seconds
    void Start()
    {
        Destroy(gameObject, destoryTime);
    }

}