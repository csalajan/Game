using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Infrastucture;
using UnityEngine;

namespace Assets.Scripts.Cameras.HUD
{
    public class FruitCollection : HudItem
    {
        protected override float posX { get { return 100.0F; } }
        protected override float posY { get { return 50.0F; } }

        private float decayRate = 15.0F;
        private float lastDecay;

        public override void SetValue()
        {
            text = String.Format("x {0}", player.GetFruit());
        }

        void Update()
        {
            if (player.GetFruit() > 0)
            {
                Decay();
                FruitLevel();
            }
            else
            {
                // Player is Dead
            }
        }

        private void Decay()
        {
            if (lastDecay.Equals(0) || (Time.time - lastDecay) > decayRate)
            {
                player.RemoveFruit(1);
                lastDecay = Time.time;
            }
        }

        private void FruitLevel()
        {
            int fruit = player.GetFruit();
            if (fruit < 15)
            {
                style.normal.textColor = Color.red;
            }
            else if (player.GetFruit() < 30)
            {
                style.normal.textColor = Color.yellow;
            }
            else
            {
                style.normal.textColor = Color.green;
            }
        }
    }
}
