namespace ISIP423_Rezantsev
{
    internal class Armor : Item
    {
        public int Defense { get; private set; }

        public Armor(string name, int defense) : base(name)
        {
            Defense = defense;
        }

        public override void DisplayStats()
        {
            Console.WriteLine($"Доспехи: {Name} (Защита: {Defense})");
        }
    }
}