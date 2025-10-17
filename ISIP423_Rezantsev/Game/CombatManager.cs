namespace ISIP423_Rezantsev
{
    internal class CombatManager
    {
        private Random random;

        public CombatManager(Random rand)
        {
            random = rand;
        }

        public bool StartCombat(Player player, Enemy enemy)
        {
            Console.WriteLine($"Вы встретили: {enemy.GetDescription()}");

            while (player.CurrentHP > 0 && enemy.CurrentHP > 0)
            {
                PlayerTurn(player, enemy);
                if (enemy.CurrentHP <= 0) break;

                EnemyTurn(player, enemy);
            }

            return player.CurrentHP > 0;
        }

        private void PlayerTurn(Player player, Enemy enemy)
        {
            // Атака / зелье
        }

        private void EnemyTurn(Player player, Enemy enemy)
        {
            // Атака 
        }
    }
}
