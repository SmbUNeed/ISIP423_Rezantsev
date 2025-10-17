using RoguelikeGame.Entities;

namespace RoguelikeGame.Items
{
    public class HealthPotion : Item
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