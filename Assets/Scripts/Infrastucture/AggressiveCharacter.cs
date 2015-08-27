using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Infrastucture
{
    public abstract class AggressiveCharacter : Character
    {
        public GameObject player;
        protected bool chasing;
        protected float attackThreshold;
        protected float giveUpThreshold;
        protected float chaseThreshold;
        protected float attackTime;

        public void CheckAggro()
        {
            var distance = (player.transform.position - transform.position).magnitude;

            if (chasing)
            {
                Target(target);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(player.transform.position - transform.position), turnSpeed*Time.deltaTime);

                if (distance > attackThreshold)
                {
                    transform.position += transform.forward*runSpeed*Time.deltaTime;
                    // play run animation
                    anim.Stop(animations.Walk);
                    anim.PlayQueued(animations.Run);
                }

                if (distance > giveUpThreshold)
                {
                    chasing = false;
                }

                if (distance < attackThreshold && Time.time > attackTime)
                {
                    //Attack
                    anim.Stop();
                    anim.PlayQueued(animations.Attack);
                    Attack(new Attack
                    {
                        Damage = 5,
                        Animation = "attack1"
                    });
                    attackTime = Time.time + attackDelay;
                }

                if (distance < attackThreshold)
                {
                    // Aggro Idle
                }
            }
            else
            {
                if (distance < chaseThreshold && !target.IsDead())
                {
                    chasing = true;
                }
            }
        }
    }
}
