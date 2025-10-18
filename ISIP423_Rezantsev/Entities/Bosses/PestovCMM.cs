namespace ISIP423_Rezantsev
{
    internal class PestovCMM : Skeleton
    {
        private double freezeChance = 0.3; // 15% + базовый шанс мага

        public PestovCMM() : base()
        {
            Name = "Пестов С-- (Босс Скелет-Маг)";
            MaxHP = (int)(MaxHP * 1.3);
            CurrentHP = MaxHP;
            Attack = (int)(Attack * 1.8);
            Defense = (int)(Defense * 0.6);
        }

        public override int CalculateDamage(Creature target)
        {
            // Сохраняет игнор защиты скелета
            return Attack;
        }

        public override void ApplySpecialEffect(Creature target)
        {
            if (random.NextDouble() < freezeChance)
            {
                Console.WriteLine("Пестов С-- использует магию заморозки! Вы пропустите следующий ход.");
            }
        }
    }
}
