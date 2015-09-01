using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Infrastucture;

namespace Assets.Scripts.Controllers
{
    public class MovementController : MonoBehaviour
    {

        private float jumpSpeed = 25.0F;
        private float gravity = 50.0F;
        private float runSpeed = 20.0F;
        private float walkSpeed = 10.0F;
        private float climbSpeed = 5.0F;
        private float rotateSpeed = 150.0F;
        private float moveSpeed = 0.0F;
        private float floatingValue = 1;
        private float slowSpeed = 1.0F;
        private float riseSpeed = 0.0F;
        private float upMove = 0.0F;

        private Transform wallTransform;

        private Animation anim;

        private CharacterController controller;

        private Character player;

        private Vector3 moveDirection = Vector3.zero;

        private string moveStatus = "idle";

        private bool grounded = false;
        private bool isWalking = true;
        private bool isJumping = false;
        private bool isClimbing = false;

        void Start()
        {
            anim = GetComponent<Animation>();
            controller = GetComponent<CharacterController>();
            player = GetComponent<Character>();

        }

        void Update()
        {
            if (!player.IsDead())
            {
                if (isClimbing)
                {
                    moveDirection = new Vector3((Input.GetMouseButton(1) ? Input.GetAxis("Horizontal") : 0),
                        Input.GetAxis("Vertical"), 0);

                    moveDirection = transform.TransformDirection(moveDirection);
                    moveDirection *= climbSpeed;
                    moveStatus = "climbing";

                }
                else
                {
                    moveDirection = new Vector3((Input.GetMouseButton(1) ? Input.GetAxis("Horizontal") : 0), 0,
                        Input.GetAxis("Vertical"));

                    moveDirection = transform.TransformDirection(moveDirection);
                    moveDirection *= isWalking ? walkSpeed : runSpeed;

                    moveStatus = "idle";
                    if (moveDirection != Vector3.zero)
                        moveStatus = isWalking ? "walking" : "running";

                    moveDirection.y += upMove;
                }
                // if moving forward and to the side at the same time, compensate for distance
//          if (Input.GetMouseButton(1) && Input.GetAxis("Horizontal") && Input.GetAxis("Vertical")) {
//              moveDirection *= .7;
//          }



                if (Input.GetButtonDown("Jump"))
                    Jump();

                if (Input.GetMouseButton(1))
                {
                    transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
                }
                else
                {
                    transform.Rotate(0, Input.GetAxis("Horizontal")*rotateSpeed*Time.deltaTime, 0);
                }


                // Toggle walking/running with the Shift key
                if (Input.GetButtonDown("Sprint"))
                    isWalking = false;

                if (Input.GetButtonUp("Sprint"))
                    isWalking = true;

                Modifiers();
                Move();
                Animate();
            }

        }

        void OnTriggerEnter(Collider other)
        {
            RaycastHit hit;
            if (other.gameObject.tag == "Climbable")
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    isClimbing = true;
                    wallTransform = other.gameObject.transform;
                    transform.rotation = Quaternion.LookRotation(-hit.normal);
                }
            }
        }

        void OnTriggerExit()
        {
            isClimbing = false;
            wallTransform = null;
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
            
            var flags = controller.Move(moveDirection*Time.deltaTime);
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
                case "climbing":
                    anim.Stop("Idle_1");
                    anim.PlayQueued("RunCycle", QueueMode.CompleteOthers);
                    break;
            }
        }
    }
}
