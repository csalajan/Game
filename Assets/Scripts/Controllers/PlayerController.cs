﻿using System;
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
        private float comboTimer = 0.0F;
        
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

                MovementModifiers(ref moveDirection);

				Walk(Input.GetAxis("Vertical") * walkSpeed, Input.GetAxis("Horizontal") * turnSpeed);
                
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    DoAttack();
                }
                comboTimer -= Time.deltaTime;


                // Targetting
                if (Input.GetButton("Target"))
                {
                    TargetClosestEnemy();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    TargetClickedEnemy();
                }

                if (Input.GetButton("Cancel"))
                {
                    ClearTarget();
                }

                
                // Jumping
                if (Input.GetButtonDown("Jump"))
                {
                    Jump(ref moveDirection);
                }   

                Move(ref moveDirection);
                upMove = moveDirection.y;
            }
        }

        private void MovementModifiers(ref Vector3 moveDirection)
        {
            if (controller.isGrounded)
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

        private void DoAttack()
        {
            if (comboTimer <= 0.0F)
            {
                Attack(Abilities[0]);
                comboTimer = 3;
            }
            else
            {
                if (lastAttack == Abilities[1])
                {
                    Attack(Abilities[2]);
                    comboTimer = 0.0F;
                }
                else
                {
                    Attack(Abilities[1]);
                    comboTimer = 3;
                }
            }
        }

        private void TargetClickedEnemy()
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

        private void Jump(ref Vector3 moveDirection)
        {
            if (controller.isGrounded)
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
