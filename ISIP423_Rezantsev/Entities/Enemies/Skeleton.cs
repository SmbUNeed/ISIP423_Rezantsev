namespace ISIP423_Rezantsev.Entities.Enemies
{
    internal class Skeleton : Enemy
    {
        public Skeleton()
        {
            Name = "Скелет";
            MaxHP = 25;
            CurrentHP = MaxHP;
            Attack = 10;
            Defense = 2;
        }

        public override void SpecialAbility(Player player)
        {
            Console.WriteLine("Скелет игнорирует защиту игрока!");
        }

        public override string GetDescription() =>
            $"Скелет (HP: {CurrentHP}, Атака: {Attack}, Защита: {Defense}, Игнор защиты)";
    }
}