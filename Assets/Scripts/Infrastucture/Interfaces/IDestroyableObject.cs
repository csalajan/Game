using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Models;

namespace Assets.Scripts.Infrastucture.Interfaces
{
    public interface IDestroyableObject
    {
        void GetHit(int damage);
        void Die();
        void Remove();
    }
}
