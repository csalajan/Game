using System;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Controllers
{
    public class MovementController : MonoBehaviour
    {

        private float jumpSpeed = 25.0F;
        private float gravity = 50.0F;
        private float runSpeed = 20.0F;
        private float walkSpeed = 10.0F;
        private float rotateSpeed = 150.0F;
        private float moveSpeed = 0.0F;
        public float floatingValue = 1;
        private float slowSpeed = 1.0F;
        private float riseSpeed = 0.0F;
        public float upMove = 0.0F;

        private Animation anim;

        private Vector3 moveDirection = Vector3.zero;

        private string moveStatus = "idle";

        private bool grounded = false;
        private bool isWalking = true;
        private bool isJumping = false;
        private bool isClimbing = false;

        void Start()
        {
            anim = GetComponent<Animation>();
        }

        void Update()
        {
            moveDirection = new Vector3((Input.GetMouseButton(1) ? Input.GetAxis("Horizontal") : 0), 0, Input.GetAxis("Vertical"));

            // if moving forward and to the side at the same time, compensate for distance
//          if (Input.GetMouseButton(1) && Input.GetAxis("Horizontal") && Input.GetAxis("Vertical")) {
//              moveDirection *= .7;
//          }

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= isWalking ? walkSpeed : runSpeed;
                
            moveStatus = "idle";
            if (moveDirection != Vector3.zero)
                moveStatus = isWalking ? "walking" : "running";

            moveDirection.y += upMove;
            
            if (Input.GetButtonDown("Jump"))
                Jump();
            
            if (Input.GetMouseButton(1))
            {
                transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            }
            else
            {
                transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime, 0);
            }

//            if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
//                Screen.lockCursor = true;
//            else
//                Screen.lockCursor = false;

            // Toggle walking/running with the Shift key
            if (Input.GetAxis("Sprint") == 1)
                isWalking = !isWalking;

            Modifiers();
            Move();
            Animate();
            
        }

        float GetSpeed()
        {
            if (moveStatus == "idle")
                moveSpeed = 0;
            if (moveStatus == "walking")
                moveSpeed = walkSpeed;
            if (moveStatus == "running")
                moveSpeed = runSpeed;
            return moveSpeed;
        }

        float GetWalkSpeed()
        {
            return walkSpeed;
        }

        void ApplyGravity()
        {
            if (!isClimbing)
            {
                var modifiedGravity = gravity;
                if (moveDirection.y <= 0)
                    modifiedGravity = gravity / floatingValue;

                moveDirection.y -= (modifiedGravity) * Time.deltaTime;
            }
        }

        void Move()
        {
            ApplyGravity();
            //Move controller
            var controller = GetComponent<CharacterController>();
            var flags = controller.Move(moveDirection * Time.deltaTime);
            grounded = (flags & CollisionFlags.Below) != 0;
            upMove = moveDirection.y;
        }

        void Jump()
        {
            if (grounded)
            {
                floatingValue = 1;
                moveDirection.y = jumpSpeed;
            }
            else
            {
                if (floatingValue == 10)
                {
                    floatingValue = 2;
                    riseSpeed = 1.0F;
                    slowSpeed = 2.0F;
                }
                else if (floatingValue == 1)
                {
                    floatingValue = 10;
                }
            }
        }

        void Modifiers()
        {
            if (grounded)
                slowSpeed = 1.0F;
            if (riseSpeed > 0.0F)
            {
                if (riseSpeed < jumpSpeed / 2.0F)
                    riseSpeed *= 2.0F;
                else
                    riseSpeed = 0.0F;
                moveDirection.y += riseSpeed;
            }
        }

        void Animate()
        {
            switch (moveStatus)
            {
                case "idle":
                    anim.Stop("RunCycle");
                    anim.PlayQueued("Idle_1", QueueMode.CompleteOthers);
                    break;
                case "walking":
                case "running":
                    anim.Stop("Idle_1");
                    anim.PlayQueued("RunCycle", QueueMode.CompleteOthers);
                    break;
            }
        }
    }
}
