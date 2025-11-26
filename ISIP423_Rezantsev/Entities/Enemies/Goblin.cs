namespace ISIP423_Rezantsev
{
    internal class Goblin : Enemy
    {
        private double critChance = 0.2; // 20% шанс крита

        public Goblin() : base("Гоблин", 30, 8, 3) { }

        public override int CalculateDamage(Creature target)
        {
            int damage = Attack;
            if (RandomSingleton.random.NextDouble() < critChance)
            {
                damage = (int)(damage * 1.5);
                Console.WriteLine("Гоблин наносит критический удар!");
            }
            return damage;
        }

        public override void ApplySpecialEffect(Creature target) { }
    }
}