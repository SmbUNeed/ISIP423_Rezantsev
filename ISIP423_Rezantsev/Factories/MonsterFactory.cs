using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP423_Rezantsev.Factories
{
    internal class MonsterFactory
    {
        public static Enemy CreateNewMonster(string monsterType)
        {
            return monsterType.ToLower() switch
            {
                "goblin" => new Goblin(),
                "skeleton" => new Skeleton(),
                "mage" => new Mage(),
                "slime" => new Slime(),
                "boss_vvg" => new VVG(),
                "boss_kovalsky" => new Kovalsky(),
                "boss_archmage" => new ArchmageCPP(),
                "boss_pestov" => new PestovCMM(),
                _ => throw new ArgumentException($"Неизвестный тип монстра: {monsterType}")
            };
        }

        public static Enemy CreateRandomMonster()
        {
            string[] monsters = { "goblin", "skeleton", "mage", "slime" };
            return CreateNewMonster(monsters[RandomSingleton.random.Next(monsters.Length)]);
        }

        public static Enemy CreateRandomBoss()
        {
            string[] bosses = { "boss_vvg", "boss_kovalsky", "boss_archmage", "boss_pestov" };
            return CreateNewMonster(bosses[RandomSingleton.random.Next(bosses.Length)]);
        }
    }
}
