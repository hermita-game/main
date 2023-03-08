using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 3f;
    public Rigidbody2D rb;
    public Animator anim;

    public KeyCode up = KeyCode.UpArrow;
    public KeyCode down = KeyCode.DownArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;

    private float actualX;
    private float actualY = 1;
    private bool diag;
    private int nbInput;
    

    private bool canDash = true;
    private bool isDashing = false;
    private float dashForce = 4f;
    private float dashTime = 0.1f;
    private float dashCoolDown = 0.3f;


    Vector2 movement;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //rb.velocity = new Vector2 (actualX * speed, actualY * speed) * Time.fixedDeltaTime;
        
    }

    public void FixedUpdate()
    {
        if (isDashing)
            return;

        if (canDash && Input.GetKey(KeyCode.LeftShift))
            StartCoroutine(Dash());

        else
        {
            //rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
            Animate2();
        }
    }

    IEnumerator Dash()
    {
        Debug.Log("Dash");
        canDash = false;
        isDashing = true;
        rb.velocity = rb.velocity * dashForce;
        /*if (anim.GetBool("isMoving"))
            rb.velocity = rb.velocity * dashForce * Time.fixedDeltaTime;*/
        //else
            //rb.velocity = new Vector2 (actualX,actualY) * dashForce * 4.5f;
        //rb(transform.position * dashForce * Time.fixedDeltaTime);
        yield return new WaitForSeconds(dashTime);
        rb.velocity = Vector2.zero;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    public void Animate2()
    {
        actualX = anim.GetFloat("MovementX");
        actualY = anim.GetFloat("MovementY");

        if (!Input.GetKey(up) && !Input.GetKey(down) && !Input.GetKey(left) && !Input.GetKey(right))
        {
            anim.SetBool("isMoving", false);
            rb.velocity = Vector2.zero;
            return;
        }

        if (anim.GetBool("isMoving"))
        {
            rb.velocity = (new Vector2(actualX, actualY/2) * speed).normalized;
        }

        if (Input.GetKey(up) && !Input.GetKey(down))
        {
            anim.SetFloat("MovementY", 1);
            anim.SetBool("isMoving", true);
        }
        if (Input.GetKey(down) && !Input.GetKey(up))
        {
            anim.SetFloat("MovementY", -1);
            anim.SetBool("isMoving", true);
        }
        if (Input.GetKey(left) && !Input.GetKey(right))
        {
            anim.SetFloat("MovementX", -1);
            anim.SetBool("isMoving", true);
        }
        if (Input.GetKey(right) && !Input.GetKey(left))
        {
            anim.SetFloat("MovementX", 1);
            anim.SetBool("isMoving", true);
        }             
        

        if (!Input.GetKey(up) && !Input.GetKey(down))
        {
            if (diag)
            {
                StartCoroutine(stayDiag());
            }               
            else
            {
                anim.SetFloat("MovementY", 0);
            }                
        }
            

        if (!Input.GetKey(left) && !Input.GetKey(right))
        {
            if (diag)
                StartCoroutine(stayDiag());
            else
            {
                anim.SetFloat("MovementX", 0);
            }                
        }

        if (actualX != 0 && actualY != 0)
        {
            diag = true;
        }
    }
    public void Animate()
    {
        if (movement.x == 0 && movement.y == 0)
        {
            anim.SetBool("isMoving", false);
        }

        else
        {            

            if (Input.GetKey(up) || Input.GetKey(down))
            {
                Debug.Log("viteuf");
                if (Input.GetKey(left) || Input.GetKey(right))
                {
                    Debug.Log("diag");
                    anim.SetBool("isMoving", true);
                    diag = true;
                    actualX = movement.x;
                    actualY = movement.y;
                }
                
                /*if (movement.x != 0 && movement.y != 0)
                {

                    diag = true;
                    actualX = movement.x;
                    actualY = movement.y;

                }*/
            }

            else
            {
                Debug.Log("uni");
                if (Input.GetKeyDown(up) || Input.GetKeyDown(down) || Input.GetKeyDown(left) || Input.GetKeyDown(right))
                {
                    anim.SetBool("isMoving", true);
                    if (diag)
                        StartCoroutine(stayDiag());
                    else
                    {
                        actualX = movement.x;
                        actualY = movement.y;
                    }
                }
            }

            anim.SetFloat("MovementX", actualX);
            anim.SetFloat("MovementY", actualY);

        }
    }

    IEnumerator stayDiag()
    {
        if(diag)
        {
            yield return new WaitForSeconds(0.05f);
            diag = false;
        }

        else
            yield break;        
    }

}
