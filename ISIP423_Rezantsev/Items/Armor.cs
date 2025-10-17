namespace ISIP423_Rezantsev
{
    internal class Armor : Item
    {
        public int Defense { get; private set; }
        public int Value { get; private set; }

        public Armor(string name, int defense, int value)
        {
            Name = name;
            Defense = defense;
            Value = value;
        }

        public override void Use(Player player)
        {
            player.EquippedArmor = this;
        }

        public override string ToString() => $"{Name} (Защита: {Defense})";
    }
}