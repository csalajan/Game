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
            healthStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.5f)) }
            };
            if (owner != null)
            {
                GUI.Box(new Rect(700, 10, healthBarBackgroundLength, 20), owner.hitPoints + "/" + owner.maxHitPoints);
                if (healthBarLength > 0)
                    GUI.Box(new Rect(700, 10, healthBarLength, 20), "", healthStyle);

            }
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
