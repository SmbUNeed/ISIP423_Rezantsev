using RoguelikeGame.Entities;

namespace RoguelikeGame.Items
{
    public abstract class Item
    {
        public string Name { get; protected set; }
        public abstract void Use(Player player);
    }
}