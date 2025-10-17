namespace ISIP423_Rezantsev
{
    internal class Weapon : Item
    {
        public int Attack { get; private set; }
        public int Value { get; private set; }

        public Weapon(string name, int attack, int value)
        {
            Name = name;
            Attack = attack;
            Value = value;
        }

        public override void Use(Player player)
        {
            player.EquippedWeapon = this;
        }

        public override string ToString() => $"{Name} (Атака: {Attack})";
    }
}