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

        private Vector3 moveDirection = Vector3.zero;
        private string moveStatus = "idle";

        private bool grounded = false;
        private bool isWalking = true;
        private bool jumping = false;
        private bool isClimbing = false;

        void Update()
        {
            // Only allow movement and jumps while grounded
            if (grounded)
            {
                moveDirection = new Vector3((Input.GetMouseButton(1) ? Input.GetAxis("Horizontal") : 0), 0, Input.GetAxis("Vertical"));

                // if moving forward and to the side at the same time, compensate for distance
                // TODO: may be better way to do this?
//                if (Input.GetMouseButton(1) && Input.GetAxis("Horizontal") && Input.GetAxis("Vertical")) {
//                    moveDirection *= .7;
//                }

                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= isWalking ? walkSpeed : runSpeed;

                moveStatus = "idle";
                if (moveDirection != Vector3.zero)
                    moveStatus = isWalking ? "walking" : "running";

                // Jump!
                if (Input.GetButton("Jump"))
                    moveDirection.y = jumpSpeed;
            }

            // Allow turning at anytime. Keep the character facing in the same direction as the Camera if the right mouse button is down.
            if (Input.GetMouseButton(1))
            {
                transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            }
            else
            {
                transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime, 0);
            }

            if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
                Screen.lockCursor = true;
            else
                Screen.lockCursor = false;

            // Toggle walking/running with the T key
            if (Input.GetAxis("Sprint") == 1)
                isWalking = !isWalking;

            //Apply gravity
            moveDirection.y -= gravity * Time.deltaTime;

            //Move controller
            var controller = GetComponent<CharacterController>();
            var flags = controller.Move(moveDirection * Time.deltaTime);
            grounded = (flags & CollisionFlags.Below) != 0;
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

        bool IsJumping()
        {
            return jumping;
        }

        float GetWalkSpeed()
        {
            return walkSpeed;
        }
    }
}
