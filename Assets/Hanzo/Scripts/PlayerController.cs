using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;

    // Start is called before the first frame update
    void Awake()
    {
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShootAnim()
    {
        playerAnim.SetBool("isShooting", true);
        playerAnim.SetBool("isRunning", false);
    }
    public void RunAnim()
    {
        playerAnim.SetBool("isShooting", !true);
        playerAnim.SetBool("isRunning", !false);
    }
}
