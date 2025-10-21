using ISIP423_Rezantsev;

namespace Pr7
{
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
        private int balance;
        private int Balance { 
            get => balance; 
            set {
                balance = value;
                if(balance < 0) LoseGame(); 
                } }
        private List<Storage> _storages = Core.Context.Storages.ToList();

        public void StartGame()
        {
            while (true)
            {
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
            return parts[random.Next(0,parts.Count)];
        }

        private void ChooseMenu(Part part)
        {
            Console.WriteLine($"Деталь: {part.Name}. Стоимость ремонта: {part.Price + part.RepairFee}.");

            Storage partStorage = _storages.FirstOrDefault(s => s.PartId == part.Id);


            Console.WriteLine($"Количество в наличии: {partStorage.Quantity}");
        }
    }
}