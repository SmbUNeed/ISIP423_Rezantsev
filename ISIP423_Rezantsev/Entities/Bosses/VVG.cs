namespace ISIP423_Rezantsev.Entities.Bosses
{
    internal class VVG : Boss
    {
        private Random random;
        private double critChance = 0.3;

        public VVG(Random rand)
        {
            random = rand;
            Name = "ВВГ";
            InitializeBoss();
        }

        protected override void InitializeBoss()
        {
            MaxHP = 30 * 2;
            CurrentHP = MaxHP;
            Attack = (int)(8 * 1.5);
            Defense = (int)(3 * 1.2);
        }

        public override void SpecialAbility(Player player)
        {
            if (random.NextDouble() < critChance)
            {
                Console.WriteLine("ВВГ наносит СМЕРТЕЛЬНЫЙ критический удар!");
            }
        }

        public override string GetDescription() =>
            $"БОСС ВВГ (HP: {CurrentHP}, Атака: {Attack}, Защита: {Defense}, Шанс крита: {critChance * 100}%)";
    }
}
