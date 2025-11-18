using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Pr8
{
    internal class Menu
    {
        public static void ShowPick(params string[] variants)
        {
            int counter = 0;
            Console.WriteLine("Выберите действие:");
            foreach(string v in variants)
            {
                counter++;
                Console.WriteLine($"{counter}. {v}");
            }
        }
        
        public static void Separator()
        {
            Console.WriteLine(new string('=', 40));
        }

        public static void Header(string headername)
        {
            Console.Clear();
            Separator();
            Console.WriteLine(headername);
            Separator();
        }
        
    }
}
