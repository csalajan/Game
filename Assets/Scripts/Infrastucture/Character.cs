using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Infrastucture.Interfaces;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Infrastucture
{
    public abstract class Character : MonoBehaviour, IDestroyableObject
    {
        public string Name { get; set; }

        public float hitPoints = 100F;
        public float maxHitPoints = 100F;
        private bool dead;
        private float deathTime;
        private float deadDuration = 10f;
        protected float attackRange = 2;
        protected float attackDelay = 3F;

        protected bool attacking;

        public Animation anim;

        public CharacterController controller;

        public Character target;
        
        protected Animations animations;

        protected Attack lastAttack;
        
        public abstract void Start();

        public abstract void Update();

        public bool IsDead()
        {
            return dead;
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
            lastAttack = attack;
        }

        public void SendHit()
        {
            
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 4, transform.forward, attackRange);

            foreach (var enemy in hits)
            {
                if (enemy.collider.tag != "Player")
                    enemy.collider.SendMessage("GetHit", lastAttack.Damage, SendMessageOptions.DontRequireReceiver);
            }
        }

        public virtual void GetHit(int damage)
        {
            if (hitPoints <= 0) return;

            hitPoints -= damage;
            anim.Stop();
            anim.PlayQueued(animations.GetHit);
            attacking = false;
            if (hitPoints <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            dead = true;
            deathTime = Time.time;
            anim.Stop();
            anim.PlayQueued(animations.Die);
        }
        
        public void Remove()
        {
            if ((Time.time - deathTime) >= deadDuration)
            {
                Destroy(gameObject);
            }
        }

        public void StartAttack()
        {
            attacking = true;
        }

        public void EndAttack()
        {
            attacking = false;
        }
    }
}
