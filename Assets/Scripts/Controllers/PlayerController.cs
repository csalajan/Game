using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Infrastucture;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class PlayerController : Character
    {
        private float upMove = 0.0F;
        public Attack[] Abilities;
        private RaycastHit hit;
        
        
        public override void Start()
        {
            animations = new Animations
            {
                Walk = "RunCycle",
                Idle = "Idle_1",
                Attack = "Attack_1",
                GetHit = "GetHit",
                Die = "Die"
            };

            Abilities = new Attack[3];
            Abilities[0] = new Attack
            {
                Name = "Attack 1",
                Animation = "Attack_1",
                Damage = 10
            };
            Abilities[1] = new Attack
            {
                Name = "Attack 2",
                Animation = "Attack_2",
                Damage = 15
            };
            Abilities[2] = new Attack
            {
                Name = "Attack 3",
                Animation = "Attack_3",
                Damage = 20
            };
        }

        public override void Update()
        {

            if (!IsDead())
            {
                var moveDirection = new Vector3(0, upMove, 0);
                float walking = Input.GetAxis("Vertical")*walkSpeed;
                float turning = Input.GetAxis("Horizontal")*turnSpeed;
                Walk(walking, turning);
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Attack(Abilities[0]);
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    Attack(Abilities[1]);
                }

                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    Attack(Abilities[2]);
                }

                if (Input.GetButton("Target"))
                {
                    TargetClosestEnemy();
                }

                if (Input.GetButton("Cancel"))
                {
                    ClearTarget();
                }

                if (Input.GetMouseButtonDown(0)) // for standalone
                {

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 1000f))
                    {
                        if (hit.collider.gameObject.tag == "Enemy")
                        {
                            Target(hit.collider.gameObject.GetComponent<Character>());
                        }
                    }
                }

                if (controller.isGrounded)
                {
                    if (Input.GetButton("Jump"))
                    {
                        moveDirection.y = jumpSpeed; // = Jump(moveDirection, jumpSpeed);
                    }
                }

                Move(ref moveDirection);
                upMove = moveDirection.y;
            }
        }

        private void TargetClosestEnemy()
        {
            GameObject closest = null;
            var distance = Mathf.Infinity;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (var enemy in enemies)
            {
                if (!enemy.GetComponent<Character>().IsDead())
                {
                    var diff = (enemy.transform.position - transform.position);
                    var curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        closest = enemy;
                        distance = curDistance;
                    }
                }
            }

            if (closest != null)
            {
                Target(closest.GetComponent<Character>());
            }
        }
    }
}
