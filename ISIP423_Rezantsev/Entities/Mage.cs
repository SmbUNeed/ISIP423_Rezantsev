namespace ISIP423_Rezantsev
{
    internal class Mage : Enemy
    {
        private Random random;
        private double freezeChance = 0.25; // 25% шанс заморозки

        public Mage(Random rand)
        {
            random = rand;
            Name = "Маг";
            MaxHP = 20;
            CurrentHP = MaxHP;
            Attack = 12;
            Defense = 1;
        }

        public override void SpecialAbility(Player player)
        {
            if (random.NextDouble() < freezeChance)
            {
                Console.WriteLine("Маг замораживает вас! Вы пропустите следующий ход.");
                player.IsFrozen = true;
            }
        }

        public override string GetDescription() =>
            $"Маг (HP: {CurrentHP}, Атака: {Attack}, Защита: {Defense}, Шанс заморозки: {freezeChance * 100}%)";
    }
}