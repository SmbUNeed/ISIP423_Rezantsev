using ISIP423_Rezantsev.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP423_Rezantsev.Items
{
    internal class HealthPotion
    {
        public PotionType Type;

        public HealthPotion(PotionType potionType)
        {
            Type = potionType;
        }
        
        public enum PotionType
        {
            Tiny = 1,
            Small = 3,
            Middle = 5,
            Big = 7,
            Great = 10
        }
    }
}
