﻿using System;
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
        public float hitPoints = 100F;
        private bool dead;
        public Animation anim;
        public CharacterController controller;
        public Character target;
        public const float gravity = 50.0F;

        public float walkSpeed = 10.0F;
        protected const float runSpeed = 30.0F;
        protected const float turnSpeed = 50.0F;
        protected const float jumpSpeed = 16.0F;

        protected float attackDelay = 10F;

        public Animations animations;

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

        protected void Target(Character character)
        {
            target = character;
        }

        protected void Attack(Attack attack)
        {
            if (target != null)
            {
                target.SendMessage("GetHit", attack.Damage);
            }
            anim.Stop();
            anim.PlayQueued(attack.Animation);
        }

        public void GetHit(float damage)
        {
            if (hitPoints <= 0) return;

            hitPoints -= damage;
            if (hitPoints <= 0)
            {
                Die();
            }
        }

        protected void Die()
        {
            
        }

        protected void Jump()
        {
           //transform.Translate(0, jumpSpeed * Time.deltaTime, 0);
            controller.Move(new Vector3(0, jumpSpeed, 0) * Time.deltaTime);
        }

        protected Vector3 Gravity(Vector3 moveDirection)
        {
            //if (controller.isGrounded)
                moveDirection.y -= gravity * Time.deltaTime;

            return moveDirection;
        }

        protected void Move(Vector3 moveDirection)
        {
            controller.Move(Gravity(moveDirection) * Time.deltaTime);
        }
    }
}
