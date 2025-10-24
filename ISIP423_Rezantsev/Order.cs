using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP423_Rezantsev
{
    internal class Order
    {
        private static int LastId = 0;

        public int Id;
        public Part Part;
        public int TurnsToDelive;
        public int PartQuantity;

        public Order(Part part, int quantity)
        {
            Id = LastId++;
            Part = part;
            PartQuantity = quantity;
            TurnsToDelive = 2;
        }
    }
}
