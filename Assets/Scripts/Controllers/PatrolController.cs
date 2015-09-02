using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Infrastucture;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Controllers
{
    public class PatrolController : AggressiveCharacter
    {
        private Vector3 waypoint;
        private int currentWaypoint;
        private bool loop = true;
        private float dampingLook = 6.0f;
        private Vector3 startPos;
        private int roamRadius = 20;
        private float idleDuration = 6.0f;
        private float curTime = 0f;
        

        public override void Start()
        {
            startPos = transform.position;
            NewWaypoint();
        }

        public override void Update()
        {
            if (!IsDead())
            {
                CheckAggro();
                if (!chasing)
                    Patrol();
            }
            else
            {
                Remove();
            }
        }

        private void Patrol()
        {
            Vector3 target = waypoint;
            target.y = transform.position.y;
            Vector3 moveDirection = target - transform.position;

            if (moveDirection.magnitude < 0.5)
            {

                if (curTime == 0)
                {
                    curTime = Time.time;
                    //Idle();
                    anim.Stop(animations.Walk);
                    anim.PlayQueued(animations.Idle);
                }

                if ((Time.time - curTime) >= idleDuration)
                {
                    NewWaypoint();
                    curTime = 0;
                }


                
            }
            else
            {
                var rotation = Quaternion.LookRotation(target - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampingLook);
                controller.Move(moveDirection.normalized * walkSpeed * Time.deltaTime);
                anim.PlayQueued(animations.Walk);
            }
        }

        private void NewWaypoint()
        {
            waypoint = new Vector3(startPos.x + Random.Range(-roamRadius, roamRadius), 0, startPos.z + Random.Range(-roamRadius, roamRadius));
        }
    }
}
