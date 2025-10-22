using ISIP423_Rezantsev;

namespace Pr7
{
    //Scaffold-DbContext "Data Source=Localhost;Initial Catalog=AutoService;Integrated Security=True;Trust Server Certificate=True" Microsoft.EntityFrameworkCore.SqlServer
    class Program
    {
        static void Main(string[] args)
        {
            Автосервис service = new();

            Console.WriteLine("Нажмите любую кнопку для начала игры...");

            service.StartNewGame();
        }
    }

    class Автосервис
    {
        private Random random = new Random();

        private int _currentTurn;

        private const int FINE = 50000;
        private const int START_BALANCE = 100000;

        private decimal balance;
        private decimal Balance { 
            get => balance; 
            set {
                balance = value;
                if (balance < 0) LoseGame(); 
                } }
        private List<Part> _parts = Core.Context.Parts.ToList();

        private List<Order> _orders = new List<Order>();

        public void StartNewGame()
        {
            WaitForUser();
            _currentTurn = 0;
            while (true)
            {
                Console.WriteLine("У вас новый клиент!");
                Part part = GetRandomPart();
                ChooseMenu(part);
            }
        }

        private void ShowOrderMenu()
        {
            Console.WriteLine("МЕНЮ ЗАКАЗА ДЕТАЛЕЙ");

            ShowAllPartsQuantity();
            while (true)
            {
                Console.WriteLine($"Введите ID детали из списка для заказа (0 - Назад)");
                int.TryParse(Console.ReadLine(), out int ans);

                Part part = _parts.FirstOrDefault(p => p.Id == ans);
                ans = -1;

                if (part != null)
                {
                    Console.WriteLine("Введите необходимое количество:");
                    int.TryParse(Console.ReadLine(), out ans);

                    decimal orderPrice = ans * part.Price;
                    Console.WriteLine($"Сумма заказа: {orderPrice} руб.");
                    if (Balance >= orderPrice)
                    {
                        Console.WriteLine("Нажмите 1 для подтверждения");
                        if (Console.ReadKey().Key == ConsoleKey.D1)
                        {
                            Balance -= orderPrice;
                            _orders.Add(new Order(part, ans));
                        }
                        else ShowOrderMenu();
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Неверный ID!");
                }
            }
        }

        private void LoseGame()
        {
            ShowBalance();
            Console.WriteLine("Вы никчемный предприниматель...");
            throw new Exception("GG");
        }

        private Part GetRandomPart()
        {
            List<Part> parts = Core.Context.Parts.ToList();
            return parts[random.Next(0, parts.Count)];
        }

        private void ShowAllPartsQuantity()
        {
            int count = 0;
            foreach (Part part in _parts)
            {
                ShowPartQuantity(part);
            }
        }

        private void ShowPartQuantity(Part part)
        {
            Console.WriteLine($"{part.Id}. {part.Name}: {part.Quantity} шт.");
        }

        private void ClaimOrder(Part part)
        {
            if(part.Quantity <= 0)
            {
                if(_parts.First(q => q.Quantity > 0) != null)
                {
                    while (true)
                    {
                        part = _parts[random.Next(0, _parts.Count)];
                        if (part.Quantity > 0) break;
                    }
                    RepairPart(part);
                    CompensateDamage(part);
                }
                else
                {
                    Console.WriteLine("На складе нет деталей!");
                    PayFine();
                }
            }
            else
            {
                Console.WriteLine("Успешная замена!");
                RepairPart(part);
            }
        }

        private void PayFine()
        {
            Console.WriteLine($"Вы оплатили штраф в размере {FINE} руб.");
            ShowBalance();
        }

        public void CompensateDamage(Part part)
        {
            decimal compensation;
            compensation = (CalculateReplacing(part) / 2) + (FINE * 2);
            Console.WriteLine($"Размер компенсации: {compensation}");
            ShowBalance();
        }

        private void RepairPart(Part part)
        {
            if(part.Quantity <= 0) { return; }
            part.Quantity -= 1;
            Balance += CalculateReplacing(part);
            Console.WriteLine($"Замена детали: {part.Name}");
            ShowBalance();
        }

        private decimal CalculateReplacing(Part part)
        {
            return part.Price + part.RepairFee;
        }

        private void CancelOrder()
        {
            Balance -= FINE;
            Console.Clear();
            Console.WriteLine($"Заказ отменен, вы оплатили штраф в размере {FINE} руб.");
            ShowBalance();
            WaitForUser();
        }

        private void ShowBalance()
        {
            Console.WriteLine($"Текущий баланс:{Balance}");
        }

        public static void WaitForUser()
        {
            Console.ReadKey();
            Console.Clear();
        }

        private void ChooseMenu(Part part)
        {
            Console.WriteLine($"Деталь: {part.Name}. Стоимость ремонта: {part.Price + part.RepairFee}.");

            Console.WriteLine("0. Заказать деталь\n1. Все детали:\n2.Принять заказ\n3.Отказаться(Штраф)");

            bool pick = true;
            while (!pick)
            {
                pick = false;
                ConsoleKey key = Console.ReadKey().Key;
                Console.Clear();
                switch (key)
                {
                    case (ConsoleKey.D0):
                        ShowOrderMenu();
                        break;
                    case (ConsoleKey.D1):
                        ShowAllPartsQuantity();
                        break;
                    case (ConsoleKey.D2):
                        ClaimOrder(part);
                        break;
                    case (ConsoleKey.D3):
                        CancelOrder();
                        break;
                    default:
                        pick = true;
                        break;
                }
            }
        }
    }
}