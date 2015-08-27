using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Infrastucture;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class PatrolController : AggressiveCharacter
    {
        public Transform[] waypoints;
        private int currentWaypoint;
        private bool loop = true;
        private float dampingLook = 6.0f;

        public override void Start()
        {
            
        }

        public override void Update()
        {
            if (currentWaypoint < waypoints.Length)
            {
                CheckAggro();
                if (!chasing)
                    Patrol();
            }
            else
            {
                if (loop)
                {
                    currentWaypoint = 0;
                }
            }
        }

        private void Patrol()
        {
            Vector3 target = waypoints[currentWaypoint].position;
            target.y = transform.position.y;
            Vector3 moveDirection = target - transform.position;

            if (moveDirection.magnitude < 0.5)
            {
                currentWaypoint++;
            }
            else
            {
                var rotation = Quaternion.LookRotation(target - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampingLook);
                controller.Move(moveDirection.normalized * walkSpeed * Time.deltaTime);
                anim.PlayQueued(animations.Walk);
            }
        }
    }
}
