using System;
using System.Collections.Generic;
using System.Linq;

namespace Wars
{
    internal class Program
    {
        static void Main()
        {
            ConsoleKey _exitButton = ConsoleKey.Enter;
            bool isWork = true;

            while (isWork)
            {
                World battleArena = new World();

                battleArena.CreateCountry();
                battleArena.CreateCountry();
                Console.ReadLine();
                Console.WriteLine("Для начало сражения нажмите на любую клавишу");
                Console.ReadKey();
                Console.Clear();

                Console.WriteLine($"\nВы хотите выйти из программы?Нажмите {_exitButton}.\nДля продолжение работы нажмите любую другую клавишу");

                if (Console.ReadKey().Key == _exitButton)
                {
                    Console.WriteLine("Вы вышли из программы");
                    isWork = false;
                }

                Console.Clear();
            }
        }
    }

    interface IWeapon
    {
        string Name { get; }

        void Attack(Unit warrior, List<Unit> targets);
    }

    class World
    {
        private List<Country> _countries = new List<Country>();

        public string NameCountry { get; private set; }

        public void CreateCountry()
        {
            Console.WriteLine("Название страны");
            NameCountry = Console.ReadLine();
            _countries.Add(new Country(NameCountry));
            CreateValueSquad();
        }

        private void CreateValueSquad()
        {
            foreach (Country country in _countries)
            {
                Console.WriteLine("Введите количество солдат в отряде");
                int.TryParse(Console.ReadLine(), out int numberUnit);
                country.CreateSquad(numberUnit);

                foreach (var squad in country.GetSquad())
                {
                    squad.CreateSoldiers(numberUnit, country.Name);
                    squad.ShowSoldiers();
                }
            }

            War war = new War(_countries);
            war.Start();
        }
    }

    class Country
    {
        private List<Squad> _squad = new List<Squad>();

        public string Name { get; }

        public Country(string name)
        {
            Name = name;
        }

        public void CreateSquad(int number)
        {
            _squad.Add(new Squad(number));
        }

        public List<Squad> GetSquad()
        {
            return _squad.ToList<Squad>();
        }
    }

    class Squad
    {
        private List<Unit> _soldiers = new List<Unit>();
        private List<IWeapon> _weapons = new List<IWeapon>() { new Rifle(50), new MachineGun(20) };
        private Random _random = new Random();

        public Squad(int number)
        {
            Number = number;
        }

        public int Number { get; }
        public int CountWarriors { get { return _soldiers.Count; } }
        public int MaxCountWarriors { get; }

        public List<Unit> GetSoldiers()
        {
            return _soldiers.ToList<Unit>();
        }

        public void CreateSoldiers(int numberSoldiersSquad, string nationality)
        {
            for (int i = 1; i <= MaxCountWarriors; i++)
            {
                _soldiers.Add(new Unit(i, numberSoldiersSquad, nationality, GiveWeapon()));
            }
        }

        public void ShowSoldiers()
        {
            foreach (var soldier in _soldiers)
            {
                soldier.ShowInfo();
            }
        }

        public void RemoveDeathSoldiers()
        {
            _soldiers.RemoveAll(soldier => soldier.Health <= 0);
        }

        private IWeapon GiveWeapon()
        {
            int numberWeapon = _random.Next(_weapons.Count);
            IWeapon weapon = _weapons[numberWeapon];
            return weapon;
        }
    }

    class War
    {
        private List<Country> _countries = new List<Country>();
        private List<Battle> _battles = new List<Battle>();

        public War(List<Country> countries)
        {
            _countries = countries;
        }

        public void Start()
        {
            for (int i = 1; GetLifeCountries().Count > 1; i++)
            {
                Console.WriteLine("Бой " + i);
                ExecuteBattle();
            }
        }

        private void ExecuteBattle()
        {
            var battle = new Battle(_countries);
            _battles.Add(battle);
            battle.ExecuteThis();
        }

        private List<Country> GetLifeCountries()
        {
            List<Country> lifeCountries = new List<Country>();

            foreach (var country in _countries)
            {
                foreach (var soldier in country.GetSquad())
                {
                    if (soldier.CountWarriors > 0)
                    {
                        lifeCountries.Add(country);
                    }
                }
            }

            return lifeCountries.ToList<Country>();
        }
    }

    class Battle
    {
        private List<Country> _countries = new List<Country>();

        public Battle(List<Country> countries)
        {
            _countries = countries;
        }

        private List<Unit> GetAllSoldiers()
        {
            List<Unit> allSoldiers = new List<Unit>();

            foreach (var country in _countries)
            {
                foreach (var platoon in country.GetSquad())
                {
                    foreach (var soldier in platoon.GetSoldiers())
                    {
                        allSoldiers.Add(soldier);
                    }
                }
            }

            return allSoldiers.ToList<Unit>();
        }

        public void ExecuteThis()
        {
            var allwarriors = GetAllSoldiers();

            foreach (var warrior in GetAllSoldiers())
            {
                warrior.Attack(allwarriors);
            }

            foreach (var country in _countries)
            {
                foreach (var squad in country.GetSquad())
                {
                    squad.RemoveDeathSoldiers();
                    Console.WriteLine($"{country.Name} - Потери: {squad.MaxCountWarriors - squad.CountWarriors} Солдат");
                    squad.ShowSoldiers();
                }
            }
        }
    }

    class Unit
    {
        public int Health;

        public int Number { get; private set; }
        public int NumberSoldier { get; private set; }
        public string Nationality { get; private set; }
        public IWeapon Weapon { get; private set; }

        public Unit(int number, int numberPlatoon, string nationality, IWeapon weapon, int health = 100)
        {
            Number = number;
            NumberSoldier = numberPlatoon;
            Nationality = nationality;
            Health = health;
            Weapon = weapon;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"{Nationality}.{NumberSoldier}.{Number}.{Weapon.Name}.{Health}");
        }

        public void Attack(List<Unit> targets)
        {
            Weapon.Attack(this, targets);
        }
    }

    class Rifle : IWeapon
    {
        public int Damage;
        private Random _random = new Random();

        public Rifle(int damage)
        {
            Damage = damage;
        }

        string IWeapon.Name { get { return "Винтовка"; } }

        public void Attack(Unit soldier, List<Unit> targets)
        {
            List<Unit> enemies = new List<Unit>();

            foreach (var target in targets)
            {
                if (soldier.Nationality != target.Nationality)
                {
                    enemies.Add(target);
                }
            }

            var targetAttack = enemies[_random.Next(enemies.Count)];

            if (targetAttack != null)
            {
                targetAttack.Health -= Damage;
                soldier.ShowInfo();
                Console.Write($" - {Damage} - ");
                targetAttack.ShowInfo();
            }
        }
    }

    class MachineGun : IWeapon
    {
        public int Damage;
        string IWeapon.Name { get { return "Пулемёт"; } }

        public MachineGun(int damage)
        {
            Damage = damage;
        }

        public void Attack(Unit warrior, List<Unit> targets)
        {
            var rifle = new Rifle(Damage);

            for (var i = 0; i < 3; i++)
            {
                rifle.Attack(warrior, targets);
            }
        }
    }
}