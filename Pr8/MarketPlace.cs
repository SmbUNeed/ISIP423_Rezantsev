using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
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
            if (CurrentUser == null)
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
            else
            {
                Menu.ShowPick("Просмотр товаров", "Корзина", "Выбрать пункт выдачи");
                while (true)
                {
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.D1:
                            ShowGoods();
                            break;
                        case ConsoleKey.D2:
                            ShowCart();
                            break;
                        case ConsoleKey.D3:
                            ChooseOffice();
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        //privet
        private void ShowCart()
        {
            Menu.Header("КОРЗИНА");

            // Исправлено: получаем товары из контекста, а не создаем пустой список
            List<Cart_Goods> cart = Core.Context.Cart_Goods.Where(cg => cg.user_id == CurrentUser.id).ToList();

            if (cart.Count == 0)
            {
                Menu.WriteRead("Корзина пуста..");
                Console.ReadKey();
                StartMenu();
                return;
            }

            List<Goods> goods = Core.Context.Goods.ToList();

            Console.WriteLine("Товары в корзине:");
            Menu.Separator();

            decimal totalPrice = 0;
            for (int i = 0; i < cart.Count; i++)
            {
                Cart_Goods c = cart[i];
                Goods product = goods.First(g => g.id == c.good_id);
                decimal itemTotal = product.price * c.good_quantity;
                totalPrice += itemTotal;

                Console.WriteLine($"{i + 1}. {product.name} | {c.good_quantity} шт. | {itemTotal} руб.");
            }

            Menu.Separator();
            Console.WriteLine($"Общая стоимость: {totalPrice} руб.");
            Menu.Separator();

            Menu.ShowPick("Заказать все товары из корзины", "Купить отдельный товар", "Очистить корзину", "Вернуться к товарам");

            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        OrderAllFromCart(cart);
                        break;
                    case ConsoleKey.D2:
                        BuySingleFromCart(cart);
                        break;
                    case ConsoleKey.D3:
                        ClearCart();
                        break;
                    case ConsoleKey.D4:
                        ShowGoods();
                        return;
                    default:
                        break;
                }
            }
        }
        private void OrderAllFromCart(List<Cart_Goods> cart)
        {
            if (CurrentUser.Office == null)
            {
                Console.WriteLine("Сначала выберите пункт выдачи!");
                ChooseOffice();
                return;
            }

            Orders order = new Orders();
            order.user_id = CurrentUser.id;
            order.office_id = CurrentUser.Office.id;
            order.date = DateTime.Now;

            Core.Context.Orders.Add(order);
            Core.Context.SaveChanges();

            List<Goods> goods = Core.Context.Goods.ToList();
            decimal totalPrice = 0;

            foreach (Cart_Goods cartItem in cart)
            {
                Orders_Goods og = new Orders_Goods();
                og.good_id = cartItem.good_id;
                og.order_id = order.id;
                og.quantity = cartItem.good_quantity;

                Goods product = goods.First(g => g.id == cartItem.good_id);
                totalPrice += product.price * cartItem.good_quantity;

                Core.Context.Orders_Goods.Add(og);
            }

            // Очищаем корзину после заказа
            Core.Context.Cart_Goods.RemoveRange(cart);
            Core.Context.SaveChanges();

            Console.WriteLine($"Заказ оформлен! Общая стоимость: {totalPrice} руб.");
            Menu.WriteRead("Нажмите любую клавишу для продолжения...");
            ShowCart();
        }
        private void BuySingleFromCart(List<Cart_Goods> cart)
        {
            try
            {
                int choice = int.Parse(Menu.WriteRead("Введите номер товара для покупки: ")) - 1;

                if (choice >= 0 && choice < cart.Count)
                {
                    Cart_Goods selectedItem = cart[choice];
                    Goods product = Core.Context.Goods.First(g => g.id == selectedItem.good_id);

                    if (CurrentUser.Office == null)
                    {
                        Console.WriteLine("Сначала выберите пункт выдачи!");
                        ChooseOffice();
                        return;
                    }

                    Orders order = new Orders();
                    order.user_id = CurrentUser.id;
                    order.office_id = CurrentUser.Office.id;
                    order.date = DateTime.Now;

                    Core.Context.Orders.Add(order);
                    Core.Context.SaveChanges();

                    Orders_Goods og = new Orders_Goods();
                    og.good_id = selectedItem.good_id;
                    og.order_id = order.id;
                    og.quantity = selectedItem.good_quantity;

                    Core.Context.Orders_Goods.Add(og);

                    // Удаляем только выбранный товар из корзины
                    Core.Context.Cart_Goods.Remove(selectedItem);
                    Core.Context.SaveChanges();

                    decimal totalPrice = product.price * selectedItem.good_quantity;
                    Console.WriteLine($"Товар '{product.name}' заказан! Стоимость: {totalPrice} руб.");
                    Menu.WriteRead("Нажмите любую клавишу для продолжения...");
                    ShowCart();
                }
                else
                {
                    Console.WriteLine("Неверный номер товара!");
                }
            }
            catch
            {
                Console.WriteLine("Ошибка ввода!");
            }
        }
        private void ClearCart()
        {
            List<Cart_Goods> userCart = Core.Context.Cart_Goods.Where(cg => cg.user_id == CurrentUser.id).ToList();

            if (userCart.Count > 0)
            {
                Core.Context.Cart_Goods.RemoveRange(userCart);
                Core.Context.SaveChanges();
                Console.WriteLine("Корзина очищена!");
            }
            else
            {
                Console.WriteLine("Корзина уже пуста!");
            }

            Menu.WriteRead("Нажмите любую клавишу для продолжения...");
            ShowCart();
        }
        private void SignUp()
        {
            Menu.Header("Регистрация");

            Users user = new Users();


            bool successSignIn = false;
            while (!successSignIn)
            {
                user.login = Menu.WriteRead("Введите логин: ");
                if (user.login != null &&
                    Core.Context.Users.ToList().FirstOrDefault(u => u.login == user.login) == null) 
                    successSignIn = true;
            }

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

            Core.Context.Users.Add(user);

            Core.Context.SaveChanges();

            Console.WriteLine("Вы зарегистрированы");

            CurrentUser = user;
            Console.ReadKey();
            ShowGoods();
        }
        private void SignIn()
        {
            while (true)
            {
                Menu.Header("ВХОД");

                string login = Menu.WriteRead("Логин: ");
                string password = Menu.WriteRead("Пароль: ");

                if (password == "" || login == "") StartMenu();

                Users user = Core.Context.Users.ToList().FirstOrDefault(u => u.login == login);

                if (user == null) continue;
                if (user.password != password)
                    Console.WriteLine("Неверный логин или пароль");
                else
                {
                    CurrentUser = user;
                    Console.WriteLine("Вход успешный!");
                    ShowGoods();
                }
            }

        }
        private void ShowGoods()
        {
            while (true)
            {
                Menu.Header("ПРОСМОТР ТОВАРОВ");

                List<Goods> goods = Core.Context.Goods.ToList();

                foreach (Goods g in goods)
                {
                    Console.WriteLine($"{g.id}. {g.name}: {g.price} руб.");
                }

                Menu.Separator();
                Console.WriteLine("0 - Вернуться в меню");
                if (CurrentUser != null)
                {
                    Console.WriteLine("9 - Просмотреть корзину");
                }
                Menu.Separator();

                string input = Menu.WriteRead("Введите ID товара или команду: ");

                if (input == "0")
                {
                    StartMenu();
                    return;
                }
                else if (CurrentUser != null && input == "9")
                {
                    ShowCart();
                    return;
                }

                int id;
                try
                {
                    id = int.Parse(input);
                }
                catch
                {
                    continue;
                }

                Goods p = goods.FirstOrDefault(g => g.id == id);
                if (p == null)
                {
                    Console.WriteLine("Неверный ID товара");
                    continue;
                }

                ProductMenu(p);
                break;
            }
        }
        private void ProductMenu(Goods product)
        {
            Menu.Header($"{product.name}");
            Console.WriteLine($"Стоимость: {product.price} руб.\nОписание: {product.description}");
            Menu.Separator();

            if (CurrentUser != null)
            {
                Menu.ShowPick("Купить", "Добавить в корзину", "Вернуться к товарам", "Просмотреть корзину");
            }
            else
            {
                Menu.ShowPick("Купить", "Добавить в корзину", "Вернуться к товарам");
            }

            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        BuyGoods(product);
                        break;
                    case ConsoleKey.D2:
                        AddToCart(product);
                        break;
                    case ConsoleKey.D3:
                        ShowGoods();
                        return;
                    case ConsoleKey.D4:
                        if (CurrentUser != null)
                        {
                            ShowCart();
                            return;
                        }
                        break;
                    case ConsoleKey.D0:
                        ShowGoods();
                        return;
                    case ConsoleKey.Enter:
                        ShowGoods();
                        return;
                    default:
                        break;
                }
            }
        }
        private void BuyGoods(Goods product)
        {
            if (!CheckSignIn())
            {
                Console.WriteLine("Для покупки необходимо войти в аккаунт!");
                StartMenu();
            }
            else
            {
                if (CurrentUser.Office == null)
                {
                    Console.WriteLine("Сначала выберите пункт выдачи!");
                    ChooseOffice();
                    return;
                }

                Orders order = new Orders();
                order.user_id = CurrentUser.id;
                order.office_id = CurrentUser.Office.id;
                order.date = DateTime.Now;

                Core.Context.Orders.Add(order);
                Core.Context.SaveChanges();

                Orders_Goods og = new Orders_Goods();
                og.good_id = product.id;
                og.order_id = order.id;

                try
                {
                    og.quantity = int.Parse(Menu.WriteRead("Введите количество товаров:"));
                }
                catch
                {
                    Console.WriteLine("Неверное количество!");
                    return;
                }

                Console.WriteLine($"Вы покупаете {og.quantity} {product.name}\nСтоимость покупки: {product.price * og.quantity}");

                Menu.WriteRead("Нажмите Enter для подтверждения покупки...");

                Core.Context.Orders_Goods.Add(og);
                Core.Context.SaveChanges();

                Console.WriteLine("Покупка совершена!");
                Menu.WriteRead("Нажмите любую клавишу для продолжения...");
                ShowGoods();
            }
        }
        private void AddToCart(Goods product)
        {
            if (!CheckSignIn())
            {
                Console.WriteLine("Для добавления в корзину необходимо войти в аккаунт!");
                StartMenu();
                return;
            }

            if (IfInCart(product) == 0)
            {
                Cart_Goods cg = new Cart_Goods();

                cg.user_id = CurrentUser.id;
                cg.good_id = product.id;
                try
                {
                    cg.good_quantity = int.Parse(Menu.WriteRead("Введите количество: "));
                }
                catch
                {
                    Console.WriteLine("Неверное количество!");
                    return;
                }

                Core.Context.Cart_Goods.Add(cg);
                Core.Context.SaveChanges();
                Console.WriteLine("Товар добавлен в корзину!");
            }
            else
            {
                Console.WriteLine("В корзине уже есть эти товары!");
            }

            Menu.WriteRead("Нажмите любую клавишу для продолжения...");
            ProductMenu(product);
        }
        private int IfInCart(Goods product)
        {
            Cart_Goods god = Core.Context.Cart_Goods.FirstOrDefault(p => p.good_id == product.id && p.user_id == CurrentUser.id);

            if (god == null) return 0;
            else return god.good_quantity;
        }
        private bool CheckSignIn()
        {
            return CurrentUser != null;
        }
        private Office ChooseOffice()
        {
            Menu.Header("ВЫБОР ПУНКТА ВЫДАЧИ");
            if (CurrentUser.Office != null)
            {
                Console.WriteLine("У вас уже есть выбранный пункт выдачи. (1 - выбрать другой)");
                if (Console.ReadKey().Key != ConsoleKey.D1) return CurrentUser.Office;
            }
            ShowAllOffices();

            int choose;
            try
            {
                choose = int.Parse(Menu.WriteRead("Выберите номер офиса:"));
            }
            catch
            {
                Console.WriteLine("Неверный ввод!");
                return ChooseOffice();
            }

            Office selectedOffice = Core.Context.Office.ToList().FirstOrDefault(o => o.id == choose);

            if (selectedOffice != null)
            {
                CurrentUser.Office = selectedOffice;
                Core.Context.SaveChanges();
                Console.WriteLine("Пункт выдачи выбран!");
            }
            else
            {
                Console.WriteLine("Неверный номер офиса!");
                return ChooseOffice();
            }

            return selectedOffice;
        }
        private void ShowAllOffices()
        {
            foreach (Office office in Core.Context.Office.ToList())
            {
                Console.WriteLine($"{office.id}. {office.adress}");
            }
        }
    }
}