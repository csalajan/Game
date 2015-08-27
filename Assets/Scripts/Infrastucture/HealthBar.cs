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

        protected float healthBarLength;
        protected float healthBarBackgroundLength;
        protected GUIStyle healthStyle;

        // Use this for initialization
        void Start()
        {
            healthBarLength = Screen.width / 6;
            healthBarBackgroundLength = Screen.width / 6;
            
        }

        // Update is called once per frame
        void Update()
        {
            AddjustCurrentHealth(0);
        }

        void OnGUI()
        {
            healthStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.5f)) }
            };
            GUI.Box(new Rect(0, 10, healthBarBackgroundLength, 20), owner.hitPoints + "/" + owner.maxHitPoints);
            if (healthBarLength > 0)
                GUI.Box(new Rect(0, 10, healthBarLength, 20), "", healthStyle);
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

        protected Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}
