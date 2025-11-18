using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Pr8
{
    internal class MarketPlace
    {
        public void StartMenu()
        {
            Menu.ShowPick("Регистрация", "Вход", "Просмотр товаров");
            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        SignUp();
                        break;
                    case ConsoleKey.D2:
                        SignIn();
                        break;
                    case ConsoleKey.D3:
                        ShowGoods();
                        break;
                    default:
                        break;
                }
            }
        }

        public void SignUp()
        {

        }

        public void SignIn()
        {

        }

        public void ShowGoods()
        {

        }
    }
}
