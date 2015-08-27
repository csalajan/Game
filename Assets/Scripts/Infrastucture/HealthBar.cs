using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Infrastucture
{
    public class HealthBar : MonoBehaviour
    {
        public int maxHealth = 100;
        public int curHealth = 100;
        public Character owner;

        public float healthBarLength;

        // Use this for initialization
        void Start()
        {
            healthBarLength = Screen.width / 6;
        }

        // Update is called once per frame
        void Update()
        {
            AddjustCurrentHealth(0);
        }

        void OnGUI()
        {
            GUI.Box(new Rect(0, 10, healthBarLength, 20), curHealth + "/" + maxHealth);
        }

        public void AddjustCurrentHealth(int adj)
        {
            owner.hitPoints += adj;

            if (owner.hitPoints < 0)
                owner.hitPoints = 0;

            if (owner.hitPoints > owner.maxHitPoints)
                owner.hitPoints = owner.maxHitPoints;

            if (maxHealth < 1)
                maxHealth = 1;

            healthBarLength = (Screen.width / 6) * (owner.hitPoints / owner.maxHitPoints);
        }
    }
}
