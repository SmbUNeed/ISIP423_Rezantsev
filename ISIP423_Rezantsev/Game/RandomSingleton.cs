using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP423_Rezantsev
{
    internal class RandomSingleton
    {
        public static Random random = new Random();

        public RandomSingleton()
        {
            if(random == null)
            {
                random = new Random();
            }
        }
    }
}
