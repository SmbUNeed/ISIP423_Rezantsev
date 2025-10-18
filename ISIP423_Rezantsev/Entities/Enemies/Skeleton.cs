namespace ISIP423_Rezantsev
{
    internal class Skeleton : Enemy
    {
        public Skeleton() : base("Скелет", 25, 10, 2) { }

        public override int CalculateDamage(Creature target) => Attack;

        public override void ApplySpecialEffect(Creature target) { }
    }
}