using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Infrastucture
{
    public class EnemyHealthBar : HealthBar
    {
        public Character player;

        void OnGUI()
        {
            if (owner != null)
                GUI.Box(new Rect(700, 10, healthBarLength, 20), owner.hitPoints + "/" + owner.maxHitPoints);
        }

        void Update()
        {
            if (player.target != null)
            {
                owner = player.target;
                AddjustCurrentHealth(0);
            }
            else
            {
                owner = null;
            }
        }
    }
}
