namespace ISIP423_Rezantsev
{
    internal abstract class Enemy : Creature
    {
        public Enemy(string name, int maxHP, int attack, int defense) : base(name, maxHP, attack, defense) { }

        public override int CalculateDamage(Creature target)
        {
            return Attack;
        }
    }
}