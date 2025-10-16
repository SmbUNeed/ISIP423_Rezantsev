using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP423_Rezantsev.Items
{
    internal class Weapon : Item
    {
        public double Damage;
        public Weapon(string name, int durability, double damage) : base(durability, name)
        {
            Name = name;
            
            Durability = durability;
            Damage = damage;
        }


    }
}
