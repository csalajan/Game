using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Assets.Scripts.Infrastucture;
using Assets.Scripts.Models;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Controllers
{
    public class PlayerController : Character
    {
        private Attack[] Abilities;
        private RaycastHit hit;
        private float comboTimer = 0.0F;
        private int fruit;
        private int coins;
        private int nuts;
        
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
                
            }
        }

        
        private void DoAttack()
        {
            if (!attacking)
            {
                if (comboTimer <= 0.0F)
                {
                    Attack(Abilities[0]);
                    comboTimer = 1.5F;
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
                        comboTimer = 1.5F;
                    }
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

        public int GetFruit()
        {
            return fruit;
        }

        public int GetCoins()
        {
            return coins;
        }

        public void CollectItem(string item)
        {
            switch (item)
            {
                case "Fruit":
                    fruit++;
                    break;
                case "Coin":
                    coins++;
                    break;
            }
        }
    }
}
