using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Items;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveH, moveV;
    [SerializeField] private float speed = 2f;
    public Inventory inventory;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        moveH = Input.GetAxisRaw("Horizontal") * speed;
        moveV = Input.GetAxisRaw("Vertical") * speed;
    }
    
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveH, moveV);
    }
    
    public Task<bool> MoveTo(float x, float y)
    {
        // TODO: Move to x, y
        // true if a path was found, false otherwise
        // Task is done when movement is complete
        return Task.FromResult(true);
    }
}
