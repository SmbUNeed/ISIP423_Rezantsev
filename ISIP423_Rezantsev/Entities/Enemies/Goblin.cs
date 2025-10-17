namespace ISIP423_Rezantsev.Entities.Enemies
{
    internal class Goblin : Enemy
    {
        private Random random;
        private double critChance = 0.2; // 20% шанс крита

        public Goblin(Random rand)
        {
            random = rand;
            Name = "Гоблин";
            MaxHP = 30;
            CurrentHP = MaxHP;
            Attack = 8;
            Defense = 3;
        }

        public override void SpecialAbility(Player player)
        {
            if (random.NextDouble() < critChance)
            {
                Console.WriteLine("Гоблин наносит критический удар!");
            }
        }

        public override string GetDescription() =>
            $"Гоблин (HP: {CurrentHP}, Атака: {Attack}, Защита: {Defense}, Шанс крита: {critChance * 100}%)";
    }
}