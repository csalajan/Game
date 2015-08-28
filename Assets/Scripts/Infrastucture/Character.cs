using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Infrastucture
{
    public abstract class Character : MonoBehaviour
    {
        public string Name { get; set; }

        public float hitPoints = 100F;
        public float maxHitPoints = 100F;
        private bool dead;
        private float deathTime;
        private float deadDuration = 10f;
        public Animation anim;
        public CharacterController controller;
        public Character target;
        public const float gravity = 50.0F;
        public float attackRange = 5;

        public float walkSpeed = 10.0F;
        protected const float runSpeed = 20.0F;
        protected const float turnSpeed = 50.0F;
        protected const float jumpSpeed = 16.0F;

        protected float attackDelay = 3F;

        public Animations animations;

        private Color[] colors;
        private bool logInitialFadeSequence = false;

        public abstract void Start();

        public abstract void Update();

        public bool IsDead()
        {
            return dead;
        }

        protected void Walk(float walkSpeed, float turnSpeed)
        {
            transform.Translate(0, 0, walkSpeed*Time.deltaTime);
            transform.Rotate(0, turnSpeed*Time.deltaTime, 0);

            if (walkSpeed > 0.5 || walkSpeed < -0.5)
            {
                anim.Stop(animations.Idle);
                anim.PlayQueued(animations.Walk);
            }
            else
            {
                anim.Stop(animations.Walk);
                anim.PlayQueued(animations.Idle);
            }
        }

        protected void Idle()
        {
            anim.Stop(animations.Walk);
            anim.PlayQueued(animations.Idle);
        }

        protected void Target(Character character)
        {
            target = character;
        }

        protected void ClearTarget()
        {
            target = null;
        }

        protected void Attack(Attack attack)
        {
            anim.Stop();
            anim.PlayQueued(attack.Animation);
            if (target != null)
            {
                if ((target.transform.position - transform.position).magnitude <= attackRange)
                {
                    target.SendMessage("GetHit", attack.Damage);
                }
            }
        }

        public void GetHit(float damage)
        {
            if (hitPoints <= 0) return;

            hitPoints -= damage;
            anim.Stop();
            anim.PlayQueued(animations.GetHit);
            if (hitPoints <= 0)
            {
                Die();
            }
        }

        protected void Die()
        {
            dead = true;
            deathTime = Time.time;
            anim.Stop();
            anim.PlayQueued(animations.Die);
        }

        protected void Jump()
        {
            //transform.Translate(0, jumpSpeed * Time.deltaTime, 0);
            controller.Move(new Vector3(0, jumpSpeed, 0)*Time.deltaTime);
        }

        protected Vector3 Gravity(ref Vector3 moveDirection)
        {

            moveDirection.y -= gravity*Time.deltaTime;

            return moveDirection;
        }

        protected void Move(ref Vector3 moveDirection)
        {
            controller.Move(Gravity(ref moveDirection)*Time.deltaTime);
        }

        public void Remove()
        {
            if ((Time.time - deathTime) >= deadDuration)
            {
                Destroy(GetComponent<GameObject>());
            }
        }
    }
}
