using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Infrastucture.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Infrastucture
{
    public abstract class DestroyableObject : MonoBehaviour, IDestroyableObject
    {
        public void GetHit(int damage)
        {
            throw new NotImplementedException();
        }

        public void Die()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
