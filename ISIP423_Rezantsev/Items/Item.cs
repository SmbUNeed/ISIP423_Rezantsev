namespace ISIP423_Rezantsev
{
    internal abstract class Item
    {
        public string Name { get; protected set; }
        public abstract void Use(Player player);
    }
}