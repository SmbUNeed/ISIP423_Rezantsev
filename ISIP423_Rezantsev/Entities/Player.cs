namespace ISIP423_Rezantsev
{
    internal class Player : Creature
    {
        public Weapon CurrentWeapon { get; private set; }
        public Armor CurrentArmor { get; private set; }
        public bool IsFrozen { get; set; }

        public Player() : base("Игрок", 100, 10, 5)
        {
            // Стартовое снаряжение
            CurrentWeapon = new Weapon("Ржавый меч", 5);
            CurrentArmor = new Armor("Кожаная броня", 3);
            UpdateStats();
        }

        private void UpdateStats()
        {
            Attack = 10 + (CurrentWeapon?.Attack ?? 0);
            Defense = 5 + (CurrentArmor?.Defense ?? 0);
        }

        public void EquipWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
            UpdateStats();
        }

        public void EquipArmor(Armor armor)
        {
            CurrentArmor = armor;
            UpdateStats();
        }

        public void UseHealingPotion()
        {
            Heal(MaxHP);
            Console.WriteLine("Вы использовали лечебное зелье! Здоровье полностью восстановлено.");
        }

        public override int CalculateDamage(Creature target)
        {
            return Attack;
        }

        public override void ApplySpecialEffect(Creature target)
        {
            // Игрок не имеет специальных эффектов
        }

        public override void DisplayStats()
        {
            Console.WriteLine($"=== ИГРОК ===");
            Console.WriteLine($"HP: {CurrentHP}/{MaxHP}");
            CurrentWeapon?.DisplayStats();
            CurrentArmor?.DisplayStats();
            Console.WriteLine($"Общая атака: {Attack}, Общая защита: {Defense}");
            Console.WriteLine("==============");
        }

        public bool TryDodge()
        {
            return random.NextDouble() < 0.4; // 40% шанс уклонения
        }

        public int CalculateBlock(int incomingDamage)
        {
            double blockPercentage = 0.7 + (random.NextDouble() * 0.3); // 70-100% защиты
            int blockedDamage = (int)(Defense * blockPercentage);
            return Math.Min(blockedDamage, incomingDamage);
        }
    }
}