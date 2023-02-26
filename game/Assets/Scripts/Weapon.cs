using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefabs;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("shot");
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefabs, firePoint.position, firePoint.rotation);
    }
}
