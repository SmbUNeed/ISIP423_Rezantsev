using ISIP423_Rezantsev.Items;

namespace RoguelikeGame.Entities
{
    public class Player
    {
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public Weapon EquippedWeapon { get; set; }
        public Armor EquippedArmor { get; set; }
        public bool IsFrozen { get; set; }

        public Player(int maxHP)
        {
            MaxHP = maxHP;
            CurrentHP = maxHP;
            IsFrozen = false;
            // Начальная экипировка
            EquippedWeapon = new Weapon("Кулаки", 2, 0);
            EquippedArmor = new Armor("Тряпки", 1, 0);
        }

        public void Heal() => CurrentHP = MaxHP;

        public int CalculateAttack() => EquippedWeapon.Attack;

        public int CalculateDefense() => EquippedArmor.Defense;
    }
}