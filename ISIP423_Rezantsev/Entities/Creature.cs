namespace ISIP423_Rezantsev
{
    internal abstract class Creature
    {
        public string Name { get; protected set; }
        public int MaxHP { get; protected set; }
        public int CurrentHP { get; protected set; }
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }

        protected Random random;

        public Creature(string name, int maxHP, int attack, int defense)
        {
            Name = name;
            MaxHP = maxHP;
            CurrentHP = maxHP;
            Attack = attack;
            Defense = defense;
            random = new Random();
        }

        public virtual void TakeDamage(int damage)
        {
            CurrentHP -= damage;
            if (CurrentHP < 0) CurrentHP = 0;
        }

        public void Heal(int amount)
        {
            CurrentHP += amount;
            if (CurrentHP > MaxHP) CurrentHP = MaxHP;
        }

        public bool IsAlive => CurrentHP > 0;

        public virtual void DisplayStats() => 
            Console.WriteLine($"{Name}: HP {CurrentHP}/{MaxHP}, Атака: {Attack}, Защита: {Defense}");
        

        public abstract int CalculateDamage(Creature target);
        public abstract void ApplySpecialEffect(Creature target);
    }
}
