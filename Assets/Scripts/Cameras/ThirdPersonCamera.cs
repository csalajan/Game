using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Cameras
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public Transform TargetLookAt;
 
        public float Distance = 20.0F;
        public float DistanceMin = 10.0F;
        public float DistanceMax = 30.0F;
 
        private float mouseX = 0.0F;
        private float mouseY = 0.0F;
        private float startingDistance = 0.0F;    
        private float desiredDistance = 0.0F;
 
        public float X_MouseSensitivity = 5.0F;
        public float Y_MouseSensitivity = 5.0F;
        public float MouseWheelSensitivity = 5.0F;
        public float Y_MinLimit = -40.0F;
        public float Y_MaxLimit = 80.0F;
 
        public float DistanceSmooth  = 0.05F;
        private float velocityDistance = 0.0F;
        private Vector3 desiredPosition = Vector3.zero;
 
        public float X_Smooth = 0.05F;
        public float Y_Smooth = 0.1F;
        private float velX = 0.0F;
        private float velY = 0.0F;
        private float velZ = 0.0F;
        private Vector3 position = Vector3.zero;

        public bool colliding;

        void Start()
        {
            Distance = Mathf.Clamp(Distance, DistanceMin, DistanceMax);
            startingDistance = Distance;
            Reset();
        }

        void LateUpdate()
        {
            if (TargetLookAt == null)
                return;

            HandlePlayerInput();

            CalculateDesiredPosition();

            UpdatePosition();
        }

        void HandlePlayerInput()
        {
            var deadZone = 0.01; // mousewheel deadZone

            if (Input.GetMouseButton(1))
            {
                mouseX += Input.GetAxis("Mouse X")*X_MouseSensitivity;
                mouseY -= Input.GetAxis("Mouse Y")*Y_MouseSensitivity;
                //}

                // this is where the mouseY is limited - Helper script
                mouseY = ClampAngle(mouseY, Y_MinLimit, Y_MaxLimit);

                // get Mouse Wheel Input
                if (Input.GetAxis("Mouse ScrollWheel") < -deadZone || Input.GetAxis("Mouse ScrollWheel") > deadZone)
                {
                    desiredDistance = Mathf.Clamp(Distance - (Input.GetAxis("Mouse ScrollWheel")*MouseWheelSensitivity),
                        DistanceMin, DistanceMax);
                }
                
            }
            else
            {
                // Slowly rotate camera behind player
            }
        }

        void OnTriggerEnter()
        {
            Debug.Log("Colliding");
            colliding = true;
        }

        void OnTriggerExit()
        {
            colliding = false;
        }

        void CalculateDesiredPosition()
        {
            // Evaluate distance
            Distance = Mathf.SmoothDamp(Distance, desiredDistance, ref velocityDistance, DistanceSmooth);

            // Calculate desired position -> Note : mouse inputs reversed to align to WorldSpace Axis
            desiredPosition = CalculatePosition(mouseY, mouseX, Distance);
        }

        Vector3 CalculatePosition(float rotationX, float rotationY , float distance)
        {
            Vector3 direction = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
            return TargetLookAt.position + (rotation * direction);
        }

        void UpdatePosition()
        {
            var posX = Mathf.SmoothDamp(position.x, desiredPosition.x, ref velX, X_Smooth);
            var posY = Mathf.SmoothDamp(position.y, desiredPosition.y, ref velY, Y_Smooth);
            var posZ = Mathf.SmoothDamp(position.z, desiredPosition.z, ref velZ, X_Smooth);
            position = new Vector3(posX, posY, posZ);

            transform.position = position;

            transform.LookAt(TargetLookAt);

            if (colliding)
            {
                desiredPosition -= new Vector3(1, 1, 1);
                UpdatePosition();
            }
        }

        void Reset()
        {
            mouseX = 0;
            mouseY = 10;
            Distance = startingDistance;
            desiredDistance = Distance;
        }

        float ClampAngle(float angle, float min, float max)
        {
            while (angle < -360 || angle > 360)
            {
                if (angle < -360)
                    angle += 360;
                if (angle > 360)
                    angle -= 360;
            }

            return Mathf.Clamp(angle, min, max);
        }
    }
}
