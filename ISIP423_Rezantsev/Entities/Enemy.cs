namespace ISIP423_Rezantsev
{
    internal abstract class Enemy
    {
        public string Name { get; protected set; }
        public int MaxHP { get; protected set; }
        public int CurrentHP { get; set; }
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }

        public abstract void SpecialAbility(Player player);
        public abstract string GetDescription();
    }
}