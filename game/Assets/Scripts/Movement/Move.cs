using System.Collections;
using UnityEngine;

namespace Movement
{
    public class Move : MonoBehaviour
    {
        public float speed = 3f;
        public Rigidbody2D rb;
        public Animator anim;

        public KeyCode up = KeyCode.UpArrow;
        public KeyCode down = KeyCode.DownArrow;
        public KeyCode left = KeyCode.LeftArrow;
        public KeyCode right = KeyCode.RightArrow;

        private float _actualX;
        private float _actualY = 1;
        private bool _diag;
        private int _nbInput;
    

        private bool _canDash = true;
        private bool _isDashing;
        private float _dashForce = 4f;
        private const float DashTime = 0.1f;
        private const float DashCoolDown = 0.3f;


        private Vector2 _movement;
        private static readonly int MovementY = Animator.StringToHash("MovementY");
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int MovementX = Animator.StringToHash("MovementX");

        // Update is called once per frame
        void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
        
        }

        public void FixedUpdate()
        {
            if (_isDashing)
                return;

            if (_canDash && Input.GetKey(KeyCode.LeftShift))
                StartCoroutine(Dash());

            else
            {
                Animate();
            }
        }

        IEnumerator Dash()
        {
            _canDash = false;
            _isDashing = true;
            yield return new WaitForSeconds(DashTime);
            var velocity = Vector2.zero;
            rb.velocity = velocity;
            _isDashing = false;
            yield return new WaitForSeconds(DashCoolDown);
            _canDash = true;
        }

        private void Animate()
        {
            _actualX = anim.GetFloat(MovementX);
            _actualY = anim.GetFloat(MovementY);

            if (!Input.GetKey(up) && !Input.GetKey(down) && !Input.GetKey(left) && !Input.GetKey(right))
            {
                anim.SetBool(IsMoving, false);
                rb.velocity = Vector2.zero;
                return;
            }

            if (anim.GetBool(IsMoving))
            {
                rb.velocity = (new Vector2(_actualX, _actualY/2) * speed).normalized;
            }

            if (Input.GetKey(up) && !Input.GetKey(down))
            {
                anim.SetFloat(MovementY, 1);
                anim.SetBool(IsMoving, true);
            }
            if (Input.GetKey(down) && !Input.GetKey(up))
            {
                anim.SetFloat(MovementY, -1);
                anim.SetBool(IsMoving, true);
            }
            if (Input.GetKey(left) && !Input.GetKey(right))
            {
                anim.SetFloat(MovementX, -1);
                anim.SetBool(IsMoving, true);
            }
            if (Input.GetKey(right) && !Input.GetKey(left))
            {
                anim.SetFloat(MovementX, 1);
                anim.SetBool(IsMoving, true);
            }
            
            if (!Input.GetKey(up) && !Input.GetKey(down))
            {
                if (_diag)
                {
                    StartCoroutine(StayDiag());
                }               
                else
                {
                    anim.SetFloat(MovementY, 0);
                }                
            }

            if (!Input.GetKey(left) && !Input.GetKey(right))
            {
                if (_diag)
                    StartCoroutine(StayDiag());
                else
                {
                    anim.SetFloat(MovementX, 0);
                }                
            }

            if (_actualX != 0 && _actualY != 0)
            {
                _diag = true;
            }
        }
    
        private IEnumerator StayDiag()
        {
            if(_diag)
            {
                yield return new WaitForSeconds(0.05f);
                _diag = false;
            }
        }

    }
}
