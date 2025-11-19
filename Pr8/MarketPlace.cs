using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Pr8
{
    internal class MarketPlace
    {
        private Users CurrentUser = null;
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

        private void SignUp()
        {
            Menu.Header("Регистрация");

            Users user = new Users();
            user.login = Menu.WriteRead("Введите логин: ");
            user.name = Menu.WriteRead("Введите имя пользователя: ");
            user.phone_number = Menu.WriteRead("Введите номер телефона: ");
            string password;
            string acceptPassword;

            do
            {
                password = Menu.WriteRead("Введите пароль: ");
                acceptPassword = Menu.WriteRead("Введите пароль повторно: ");

                if (password != acceptPassword)
                {
                    Console.WriteLine("Пароли не совпадают");

                }
            } while (password != acceptPassword);

            user.password = password;
            user.Office = ChooseOffice();

            Core.Context.Users.Add(user);

            Core.Context.SaveChanges();

            Console.WriteLine("Вы зарегистрированы");

            CurrentUser = user;
        }

        private void SignIn()
        {
            List<Users> users = Core.Context.Users.ToList();
            while (true) 
            {
                Menu.Header("ВХОД");
            
                string login = Menu.WriteRead("Логин: ");
                string password = Menu.WriteRead("Пароль: ");

                if (password == "" || login == "") StartMenu();

                Users user = users.FirstOrDefault(u => u.login == login);

                if (user == null) continue;
                if (user.password != password) 
                    Console.WriteLine("Неверный логин или пароль");
                else
                {
                    CurrentUser = user;
                    Console.WriteLine("Вход успешный!");
                    break;
                }
            }
        }

        private void ShowGoods()
        {

        }

        private Office ChooseOffice()
        {
            Menu.Header("Выбор пункта выдачи");
            ShowAllOffice();

            int choose = int.Parse(Menu.WriteRead("Выберите номер офиса:"));


            return Core.Context.Office.ToList()
                .FirstOrDefault(o => o.id == choose);
        }

        private void ShowAllOffice()
        {
            foreach (Office office in Core.Context.Office.ToList())
            {
                Console.WriteLine($"{office.id}. {office.adress}");
            }
        }
    }
}
