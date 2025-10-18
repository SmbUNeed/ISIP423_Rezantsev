using System;
using System.Collections.Generic;

namespace ISIP423_Rezantsev
{
    internal abstract class Item
    {
        public string Name { get; protected set; }

        public Item(string name)
        {
            Name = name;
        }

        public abstract void DisplayStats();
    }
}