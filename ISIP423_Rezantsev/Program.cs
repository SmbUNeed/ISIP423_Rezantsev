using ISIP423_Rezantsev;

namespace Pr7
{
    //Scaffold-DbContext "Data Source=Localhost;Initial Catalog=AutoService;Integrated Security=True;Trust Server Certificate=True" Microsoft.EntityFrameworkCore.SqlServer
    class Program
    {
        static void Main(string[] args)
        {
            Автосервис service = new();
            service.StartGame();
        }
    }

    class Автосервис
    {
        private Random random = new Random();

        private const int FINE = 50000;
        private decimal balance;
        private decimal Balance { 
            get => balance; 
            set {
                balance = value;
                if (balance < 0) LoseGame(); 
                } }
        private List<Part> _parts = Core.Context.Parts.ToList();

        public void StartGame()
        {
            while (true)
            {
                Console.WriteLine("У вас новый клиент!");
                Part part = GetRandomPart();
                ChooseMenu(part);
            }

        }

        private void LoseGame()
        {
            Console.WriteLine("Ты никчемный предприниматель");
            throw new Exception("GG");
        }

        private Part GetRandomPart()
        {
            List<Part> parts = Core.Context.Parts.ToList();
            return parts[random.Next(0, parts.Count)];
        }

        private void ShowAllPartsQuantity()
        {
            foreach (Part part in _parts)
            {
                ShowPartQuantity(part);
            }
        }

        private void ShowPartQuantity(Part part)
        {
            Console.WriteLine($"{part.Name}: {part.Quantity} шт.");
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
                }
            }
        }

        public void CompensateDamage(Part part)
        {
            Balance -= (CalculateReplacing(part) / 2) + (FINE * 2);
        }

        private void RepairPart(Part part)
        {
            if(part.Quantity <= 0) { return; }
            part.Quantity -= 1;
            Balance += CalculateReplacing(part);
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
        }

        private void ChooseMenu(Part part)
        {
            Console.WriteLine($"Деталь: {part.Name}. Стоимость ремонта: {part.Price + part.RepairFee}.");

            Console.WriteLine("1. Все детали:\n2.Принять заказ\n3.Отказаться(Штраф)");

            bool pick = true;
            while (!pick)
            {
                pick = false;
                switch (Console.ReadKey().Key)
                {
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