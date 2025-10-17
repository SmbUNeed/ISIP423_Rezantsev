namespace ISIP423_Rezantsev
{
    internal class GameManager
    {
        private Player player;
        private Random random;
        private int turnCount;

        public GameManager()
        {
            random = new Random();
            player = new Player(100);
            turnCount = 0;
        }

        public void StartGame()
        {
            Console.WriteLine("Стартуем");

            while (player.CurrentHP > 0)
            {
                turnCount++;
                Console.WriteLine($"\n--- Ход {turnCount} ---");

                if (turnCount % 10 == 0)
                {
                    // Бой с боссом
                    EncounterBoss();
                }
                else
                {
                    // Обычный ход: 50/50 сундук или враг
                    if (random.Next(2) == 0)
                    {
                        EncounterEnemy();
                    }
                    else
                    {
                        OpenChest();
                    }
                }

                // Проверка на заморозку
                if (player.IsFrozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход!");
                    player.IsFrozen = false;
                    continue;
                }
            }

            Console.WriteLine("Игра окончена! Вы погибли...");
        }

        private void EncounterEnemy() { /* Логика встречи с врагом */ }
        private void EncounterBoss() { /* Логика встречи с боссом */ }
        private void OpenChest() { /* Логика открытия сундука */ }
    }
}
