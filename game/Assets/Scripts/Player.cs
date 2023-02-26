using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSample : MonoBehaviour
{
    [SerializeField]
    public float speed = 30f;
    public Rigidbody2D rb;
    private float horizontal;
    private float vertical;
    public Image bar1;
    public Image bar2;
    public Image bar3;
    private float hp = 300;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");        
    }

    public void Move(Vector3 movement)
    {
        rb.MovePosition(transform.position + movement * (Time.deltaTime * speed));
    }

    public void FixedUpdate()
    {
        var movement = new Vector3(horizontal, vertical, 0).normalized;
        Move(movement);

        if(Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(float amount)
    {
        if(hp>200)
        {
            bar1.fillAmount -= amount / 100;
            hp -= amount;
        }   
        else if (hp>100)
        {
            bar2.fillAmount -= amount / 100;
            hp -= amount;
        }
        else if (hp > 0)
        {
            bar3.fillAmount -= amount / 100;
            hp -= amount;
        }
        else
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            Object.Destroy(p);
        }        
    }
}
