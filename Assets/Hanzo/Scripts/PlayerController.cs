using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;
    [SerializeField]
    private GameObject bulletGO;
    [SerializeField]
    private Transform bulletSpawnPos;
    public float bulletSpeed = 13f;
    bool isShootingOn = false;

    [SerializeField]
    private Transform playerSpawnCenter;
    private float alignToCenterSpeed;


    // Start is called before the first frame update
    void Awake()
    {
        playerAnim = GetComponent<Animator>();
        playerSpawnCenter = transform.parent.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, playerSpawnCenter.position, Time.fixedDeltaTime + alignToCenterSpeed);

    }

    public void StartShooting()
    {
        ShootAnim();
        isShootingOn = true;
        StartCoroutine(Shooting());
    }

    public void StopShooting()
    {
        isShootingOn = false;
        RunAnim();
    }

    void ShootAnim()
    {
        playerAnim.SetBool("isShooting", true);
        playerAnim.SetBool("isRunning", false);

        // Shoot();
    }

    void RunAnim()
    {
        playerAnim.SetBool("isShooting", false);
        playerAnim.SetBool("isRunning", true);
    }

    IEnumerator Shooting()
    {
        while (isShootingOn)
        {
            Shoot();
            yield return new WaitForSeconds(2f);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletGO, bulletSpawnPos.position, Quaternion.identity);
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.velocity = transform.forward * bulletSpeed;
    }



}
