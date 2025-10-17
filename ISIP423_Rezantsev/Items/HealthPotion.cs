namespace ISIP423_Rezantsev
{
    internal class HealthPotion : Item
    {
        public HealthPotion()
        {
            Name = "Лечебное зелье";
        }

        public override void Use(Player player)
        {
            player.Heal();
            Console.WriteLine("Вы выпили зелье и полностью восстановили здоровье!");
        }
    }
}