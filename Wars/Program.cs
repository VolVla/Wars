using System;
using System.Collections.Generic;

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
                Battlefild battlefild = new Battlefild();
                Console.WriteLine("Для начало сражения нажмите на любую клавишу");
                Console.ReadKey();

                battlefild.Battle();

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

    class Country
    {
        private Squad _squad;

        public Country()
        {
            Console.WriteLine("Введите название страны");
            Name = Console.ReadLine();
            Console.WriteLine("Введите количество солдат");
            int.TryParse(Console.ReadLine(), out int numberSoldiers);
            _squad = new Squad(numberSoldiers, Name);
        }

        public string Name { get; }

        public Squad GetSquad()
        {
            return _squad;
        }
    }

    class Battlefild
    {
        private int _numberFirstCountry = 0;
        private int _numberSecondCountry = 1;
        private List<Country> _countries = new List<Country>()
        {
             new Country(),
             new Country(),
        };

        public void Battle()
        {
            while (_countries[_numberFirstCountry].GetSquad().IsDeadSquad != true && _countries[_numberSecondCountry].GetSquad().IsDeadSquad != true)
            {
                _countries[_numberFirstCountry].GetSquad().AttackSquad(_countries[_numberSecondCountry].GetSquad());
                _countries[_numberSecondCountry].GetSquad().AttackSquad(_countries[_numberFirstCountry].GetSquad());
            }

            ResultBattle();
        }

        private void ResultBattle()
        {
            if (_countries[_numberFirstCountry].GetSquad().IsDeadSquad == true & _countries[_numberSecondCountry].GetSquad().IsDeadSquad == true)
            {
                Console.WriteLine("Отряды по убивали друг друга, никто не победил");
            }
            else if (_countries[_numberFirstCountry].GetSquad().IsDeadSquad == false & _countries[_numberSecondCountry].GetSquad().IsDeadSquad == true)
            {
                Console.WriteLine($"Победила страна {_countries[_numberFirstCountry].Name}");
            }
            else if (_countries[_numberFirstCountry].GetSquad().IsDeadSquad == true & _countries[_numberSecondCountry].GetSquad().IsDeadSquad == false)
            {
                Console.WriteLine($"Победила страна {_countries[_numberSecondCountry].Name}");
            }
        }
    }

    class Squad
    {
        public bool IsDeadSquad = false;
        private List<Unit> _soldiers = new List<Unit>();
        private List<IWeapon> _weapons = new List<IWeapon>() { new Rifle(50), new MachineGun(20) };
        private Random _random = new Random();

        public Squad(int number, string name)
        {
            Number = number;
            CreateSoldiers(number, name);
            ShowSoldiers();
        }

        public int Number { get; }

        public void AttackSquad(Squad squad)
        {
            int numberUnitAttackSquad = _random.Next(0, _soldiers.Count);

            for (int i = 0; i < numberUnitAttackSquad; i++)
            {
                _soldiers[i].Attack(squad.GetSoldiers());
            }

            squad.RemoveDeathSoldiers();

            if (_soldiers.Count == 0)
            {
                IsDeadSquad = true;
            }
        }

        private void ShowSoldiers()
        {
            foreach (var soldier in _soldiers)
            {
                soldier.ShowInfo();
            }
        }

        private void CreateSoldiers(int numberSoldiersSquad, string nationality)
        {
            for (int i = 1; i <= Number; i++)
            {
                _soldiers.Add(new Unit(i, numberSoldiersSquad, nationality, GiveWeapon()));
            }
        }

        private void RemoveDeathSoldiers()
        {
            _soldiers.RemoveAll(soldier => soldier.Health <= 0);
            Console.WriteLine("Убрали с поля боя мертвых солдат");
        }

        private List<Unit> GetSoldiers()
        {
            return _soldiers;
        }

        private IWeapon GiveWeapon()
        {
            int numberWeapon = _random.Next(_weapons.Count);
            IWeapon weapon = _weapons[numberWeapon];
            return weapon;
        }
    }

    class Unit
    {
        public int Health;

        public Unit(int number, int numberPlatoon, string nationality, IWeapon weapon, int health = 100)
        {
            Number = number;
            NumberSoldier = numberPlatoon;
            Nationality = nationality;
            Health = health;
            Weapon = weapon;
        }

        public int Number { get; private set; }
        public int NumberSoldier { get; private set; }
        public string Nationality { get; private set; }
        public IWeapon Weapon { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"Национальность - {Nationality}, номер солдата - {Number}, вооруженме -{Weapon.Name}, здоровье - {Health}");
        }

        public void Attack(List<Unit> targets)
        {
            Weapon.Attack(this, targets);
        }
    }

    class Rifle : IWeapon
    {
        private int _damage;

        public Rifle(int damage)
        {
            _damage = damage;
        }

        string IWeapon.Name { get { return "Винтовка"; } }

        public void Attack(Unit soldier, List<Unit> targets)
        {
            foreach (Unit target in targets)
            {
                if (targets != null)
                {
                    target.Health -= _damage;
                    soldier.ShowInfo();
                    Console.Write($"Атаковал солдата на - {_damage} - ");
                    target.ShowInfo();
                }
            }
        }
    }

    class MachineGun : IWeapon
    {
        private int _damage;

        public MachineGun(int damage)
        {
            _damage = damage;
        }

        string IWeapon.Name { get { return "Пулемёт"; } }

        public void Attack(Unit warrior, List<Unit> targets)
        {
            var rifle = new Rifle(_damage);
            int maximumTargets = 3;

            for (var i = 0; i < maximumTargets; i++)
            {
                rifle.Attack(warrior, targets);
            }
        }
    }
}