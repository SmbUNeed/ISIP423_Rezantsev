namespace ISIP423_Rezantsev
{
    internal class Game
    {
        private Player player;
        private Random random;
        private int turnCount;

        private List<Enemy>? normalEnemies;
        private List<Enemy>? bosses;

        public Game()
        {
            player = new Player();
            random = new Random();
            turnCount = 0;

            InitializeEnemies();
        }

        private void InitializeEnemies()
        {
            normalEnemies = new List<Enemy>
            {
                new Goblin(),
                new Skeleton(),
                new Mage()
            };

            bosses = new List<Enemy>
            {
                new VVG(),
                new Kovalsky(),
                new ArchmageCPP(),
                new PestovCMM()
            };
        }

        public void StartGame()
        {
            Console.WriteLine("Добро пожаловать в текстовый рогалик!");
            Console.WriteLine("Каждый ход вы будете встречать либо сундук, либо врага.");
            Console.WriteLine("Каждые 10 ходов вас ждет встреча с боссом!\n");

            while (player.IsAlive)
            {
                turnCount++;
                Console.WriteLine($"\n=== ХОД {turnCount} ===");
                player.DisplayStats();

                if (player.IsFrozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход!");
                    player.IsFrozen = false;
                    ContinueGame();
                    continue;
                }

                if (random.Next(2) == 0)
                {
                    EncounterChest();
                }
                else
                {
                    EncounterEnemy();
                }

                if (!player.IsAlive)
                {
                    Console.WriteLine("\n=== ИГРА ОКОНЧЕНА ===");
                    Console.WriteLine($"Вы продержались {turnCount} ходов.");
                    break;
                }

                ContinueGame();
            }
        }

        private void EncounterChest()
        {
            Console.WriteLine("\nВы нашли сундук!");
            int chestType = random.Next(3); // 0 - зелье, 1 - оружие, 2 - доспехи

            switch (chestType)
            {
                case 0:
                    Console.WriteLine("В сундуке лечебное зелье!");
                    player.UseHealingPotion();
                    break;

                case 1:
                    Console.WriteLine("В сундуке новое оружие!");
                    Weapon newWeapon = GenerateRandomWeapon();
                    newWeapon.DisplayStats();
                    Console.WriteLine("\nВаше текущее оружие:");
                    player.CurrentWeapon.DisplayStats();

                    Console.Write("\nХотите взять новое оружие? (y/n): ");
                    if (Console.ReadLine()?.ToLower() == "y")
                    {
                        player.EquipWeapon(newWeapon);
                        Console.WriteLine("Оружие экипировано!");
                    }
                    else
                    {
                        Console.WriteLine("Вы оставили оружие в сундуке.");
                    }
                    break;

                case 2:
                    Console.WriteLine("В сундуке новые доспехи!");
                    Armor newArmor = GenerateRandomArmor();
                    newArmor.DisplayStats();
                    Console.WriteLine("\nВаши текущие доспехи:");
                    player.CurrentArmor.DisplayStats();

                    Console.Write("\nХотите взять новые доспехи? (y/n): ");
                    if (Console.ReadLine()?.ToLower() == "y")
                    {
                        player.EquipArmor(newArmor);
                        Console.WriteLine("Доспехи экипированы!");
                    }
                    else
                    {
                        Console.WriteLine("Вы оставили доспехи в сундуке.");
                    }
                    break;
            }
        }

        private void EncounterEnemy()
        {
            Enemy enemy;

            if (turnCount % 10 == 0)
            {
                enemy = bosses[random.Next(bosses.Count)];
                Console.WriteLine($"\nВНИМАНИЕ! Появился босс - {enemy.Name}!");
            }
            else
            {
                enemy = normalEnemies[random.Next(normalEnemies.Count)];
                Console.WriteLine($"\nПоявился враг - {enemy.Name}!");
            }

            enemy.DisplayStats();
            Combat(enemy);
        }

        private void Combat(Enemy enemy)
        {
            while (player.IsAlive && enemy.IsAlive)
            {
                Console.WriteLine("\nВаш ход:");
                Console.WriteLine("1 - Атаковать");
                Console.WriteLine("2 - Защищаться");
                Console.Write("Выберите действие: ");

                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    int damage = player.CalculateDamage(enemy);
                    enemy.TakeDamage(damage);
                    Console.WriteLine($"Вы нанесли {damage} урона {enemy.Name}!");
                }
                else if (choice == "2")
                {
                    Console.WriteLine("Вы готовитесь к защите...");
                }
                else
                {
                    Console.WriteLine("Неверный выбор, вы пропускаете ход!");
                }

                if (!enemy.IsAlive)
                {
                    Console.WriteLine($"\n{enemy.Name} побежден!");
                    break;
                }

                Console.WriteLine($"\nХод {enemy.Name}:");

                bool playerDefended = choice == "2";
                int enemyDamage = enemy.CalculateDamage(player);

                if (playerDefended)
                {
                    if (player.TryDodge())
                    {
                        Console.WriteLine("Вы успешно уклонились от атаки!");
                        enemyDamage = 0;
                    }
                    else
                    {
                        int blockedDamage = player.CalculateBlock(enemyDamage);
                        enemyDamage -= blockedDamage;
                        Console.WriteLine($"Вы блокировали {blockedDamage} урона!");
                    }
                }

                if (enemyDamage > 0)
                {
                    player.TakeDamage(enemyDamage);
                    Console.WriteLine($"{enemy.Name} наносит вам {enemyDamage} урона!");
                }

                enemy.ApplySpecialEffect(player);

                Console.WriteLine($"\nВаше HP: {player.CurrentHP}/{player.MaxHP}");
                Console.WriteLine($"HP {enemy.Name}: {enemy.CurrentHP}/{enemy.MaxHP}");
            }
        }

        private Weapon GenerateRandomWeapon()
        {
            string[] weaponNames = { "Стальной меч", "Секира воина", "Кинжал убийцы", "Посох мага", "Лук охотника" };
            string name = weaponNames[random.Next(weaponNames.Length)];
            int attack = random.Next(5, 16);

            return new Weapon(name, attack);
        }

        private Armor GenerateRandomArmor()
        {
            string[] armorNames = { "Кольчуга", "Латные доспехи", "Кожаный доспех", "Мантия мага", "Доспех берсерка" };
            string name = armorNames[random.Next(armorNames.Length)];
            int defense = random.Next(3, 11);

            return new Armor(name, defense);
        }

        private void ContinueGame()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
