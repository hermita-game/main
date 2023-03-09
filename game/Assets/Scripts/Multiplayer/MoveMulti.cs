using System;
using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace Multiplayer
{
    public class MoveMulti : NetworkBehaviour
    {
        public float speed = 3f;
        public Rigidbody2D rb;
        public NetworkRigidbody2D netRb;
        public Animator anim;
        public NetworkAnimator netAnim;

        private bool _diag;
        private int _nbInput;

        private Vector3 _movement;
        private static readonly int MovementY = Animator.StringToHash("MovementY");
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int MovementX = Animator.StringToHash("MovementX");

        private void Start()
        {
            if (!IsOwner)
                transform.GetChild(0).gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsOwner) return;
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical")/2;
        }

        public void FixedUpdate()
        {
            if (!IsOwner) return;
            netRb.transform.position = transform.position + _movement.normalized * (Time.deltaTime * speed);
            Animate();
        }

        private void Animate()
        {
            anim.SetFloat(MovementX, _movement.x);
            anim.SetFloat(MovementY, _movement.y);
            anim.SetBool(IsMoving, _movement != Vector3.zero);
        }
    }
}
