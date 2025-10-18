namespace ISIP423_Rezantsev
{
    internal class VVG : Goblin
    {
        public VVG() : base()
        {
            Name = "ВВГ (Босс Гоблин)";
            MaxHP = (int)(MaxHP * 2.0);
            CurrentHP = MaxHP;
            Attack = (int)(Attack * 1.5);
            Defense = (int)(Defense * 1.2);
        }

        public override int CalculateDamage(Creature target)
        {
            int damage = Attack;
            if (random.NextDouble() < 0.3) // +10% к базовому шансу гоблина
            {
                damage = (int)(damage * 1.5);
                Console.WriteLine("ВВГ наносит сокрушительный критический удар!");
            }
            return damage;
        }
    }
}
