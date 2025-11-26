namespace ISIP423_Rezantsev
{
    internal class Mage : Enemy
    {
        private double freezeChance = 0.15; // 15% шанс заморозки

        public Mage() : base("Маг", 20, 12, 1) { }

        public override int CalculateDamage(Creature target)
        {
            return Attack;
        }

        public override void ApplySpecialEffect(Creature target)
        {
            if (RandomSingleton.random.NextDouble() < freezeChance)
            {
                Console.WriteLine("Маг замораживает вас! Вы пропустите следующий ход.");
                // Эффект заморозки будет обработан в классе игры
            }
        }
    }
}