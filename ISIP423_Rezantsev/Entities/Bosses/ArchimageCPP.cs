namespace ISIP423_Rezantsev
{
    internal class ArchmageCPP : Mage
    {
        public ArchmageCPP() : base()
        {
            Name = "Архимаг C++ (Босс Маг)";
            MaxHP = (int)(MaxHP * 1.8);
            CurrentHP = MaxHP;
            Attack = (int)(Attack * 1.6);
            Defense = (int)(Defense * 1.1);
        }

        public override void ApplySpecialEffect(Creature target)
        {
            if (RandomSingleton.random.NextDouble() < 0.25) // +10% к базовому шансу мага
            {
                Console.WriteLine("Архимаг C++ накладывает мощную заморозку! Вы пропустите следующий ход.");
            }
        }
    }
}
