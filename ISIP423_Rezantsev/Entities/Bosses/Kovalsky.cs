namespace ISIP423_Rezantsev.Entities.Bosses
{
    internal class Kovalsky: Boss
    {
        public Kovalsky()
        {
            Name = "Ковальский";
            InitializeBoss();
        }

        protected override void InitializeBoss()
        {
            MaxHP = (int)(25 * 2.5);
            CurrentHP = MaxHP;
            Attack = (int)(10 * 1.3);
            Defense = (int)(2 * 1.4);
        }

        public override void SpecialAbility(Player player)
        {
            Console.WriteLine("КОВАЛЬ(ЕВ)СКИЙ ИГНОРИРУЕТ ЗАЩИТУ ИГРОКА!");
        }

        public override string GetDescription() =>
            $"БОСС Ковальский (HP: {CurrentHP}, Атака: {Attack}, Защита: {Defense}, Игнор защиты)";
    }
}
