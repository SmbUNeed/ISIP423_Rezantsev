namespace ISIP423_Rezantsev.Entities.Bosses
{
    internal class PestovCMM : Boss
    {
        private Random random;
        private double freezeChance = 0.25 + 0.15;

        public PestovCMM(Random rand)
        {
            random = rand;
            Name = "Пестов С--";
            InitializeBoss();
        }

        protected override void InitializeBoss()
        {
            MaxHP = (int)(25 * 1.3);
            CurrentHP = MaxHP;
            Attack = (int)(10 * 1.8);
            Defense = (int)(2 * 0.6);
        }

        public override void SpecialAbility(Player player)
        {
            Console.WriteLine("ПЕСТОВ ИГНОРИРУЕТ ЗАЩИТУ ИГРОКА!");
            if (random.NextDouble() < freezeChance)
            {
                Console.WriteLine("Пестов замораживает вас! Вы пропустите следующий ход.");
                player.IsFrozen = true;
            }
        }

        public override string GetDescription() =>
            $"БОСС Пестов С-- (HP: {CurrentHP}, Атака: {Attack}, Защита: {Defense}, Игнор защиты)";
    }
}
