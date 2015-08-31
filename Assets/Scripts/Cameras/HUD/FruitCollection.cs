using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Infrastucture;

namespace Assets.Scripts.Cameras.HUD
{
    public class FruitCollection : HudItem
    {
        protected override float posX { get { return 100.0F; } }
        protected override float posY { get { return 50.0F; } }

        public override void SetValue()
        {
            text = String.Format("x {0}", player.GetFruit());
        }
    }
}
