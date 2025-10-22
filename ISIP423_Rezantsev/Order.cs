using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP423_Rezantsev
{
    internal class Order
    {
        public Part Part;
        public int TurnsToDelive;
        public int PartQuantity;

        public Order(Part part, int quantity)
        {
            Part = part;
            PartQuantity = quantity;
            TurnsToDelive = 2;
        }
    }
}
