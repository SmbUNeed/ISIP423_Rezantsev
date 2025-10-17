namespace ISIP423_Rezantsev.Entities.Bosses
{
    internal class ArchimageCPP : Boss
    {
        private Random random;
        private double freezeChance = 0.25 + 0.10;

        public ArchimageCPP(Random rand)
        {
            random = rand;
            Name = "Архимаг С++";
            InitializeBoss();
        }

        protected override void InitializeBoss()
        {
            MaxHP = (int)(20 * 1.8);
            CurrentHP = MaxHP;
            Attack = (int)(12 * 1.6);
            Defense = (int)(1 * 1.1);
        }

        public override void SpecialAbility(Player player)
        {
            if (random.NextDouble() < freezeChance)
            {
                Console.WriteLine("Архимаг замораживает вас! Вы пропустите следующий ход.");
                player.IsFrozen = true;
            }
        }

        public override string GetDescription() =>
            $"БОСС Архимаг С++ (HP: {CurrentHP}, Атака: {Attack}, Защита: {Defense}, Шанс заморозки: {freezeChance})";
    }
}
