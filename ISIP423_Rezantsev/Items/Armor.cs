using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ISIP423_Rezantsev.Items
{
    internal class Armor : Item
    {
        public double Protection;

        public Armor(string name, int durability, double protection) : base(durability, name)
        {
            Name = name;

            Durability = durability;
            Protection = protection;
        }
    }
}
