using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP423_Rezantsev.Items
{
    internal abstract class Item
    {
        protected int Durability;
        public string? Name;

        protected Item(int durability, string name)
        {
            Durability = durability;
            Name = name;
        }

        public bool BrokeCheck()
        {
            return Durability <= 0;
        }
    }
}
