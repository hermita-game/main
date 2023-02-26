using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField]
    public float speed = 10f;
    public Rigidbody2D rb;
    public NetworkRigidbody2D netRb;
    private float _horizontal;
    private float _vertical;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        netRb = GetComponent<NetworkRigidbody2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");        
    }

    private void Move(Vector3 movement)
    {
        netRb.transform.position = transform.position + movement * (Time.deltaTime * speed);
    }

    public void FixedUpdate()
    {
        if (!IsOwner) return;
        var movement = new Vector3(_horizontal, _vertical, 0).normalized;
        Move(movement);
    }
}
