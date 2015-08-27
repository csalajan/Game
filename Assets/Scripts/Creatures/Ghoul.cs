using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Controllers;
using Assets.Scripts.Models;

namespace Assets.Creatures
{
    public class Ghoul : PatrolController
    {
        public override void Start()
        {
            animations = new Animations
            {
                Walk = "walk",
                Run = "run",
                Attack = "attack1"
            };
        }
    }
}
