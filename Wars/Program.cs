using System;
using System.Collections.Generic;

namespace Wars
{
    internal class Program
    {
        static void Main()
        {
            ConsoleKey exitButton = ConsoleKey.Enter;
            bool isWork = true;

            while (isWork)
            {
                Battlefild battlefild = new Battlefild();
                Console.WriteLine("Для начало сражения нажмите на любую клавишу");
                Console.ReadKey();

                battlefild.Battle();

                Console.WriteLine($"\nВы хотите выйти из программы?Нажмите {exitButton}.\nДля продолжение работы нажмите любую другую клавишу");

                if (Console.ReadKey().Key == exitButton)
                {
                    Console.WriteLine("Вы вышли из программы");
                    isWork = false;
                }

                Console.Clear();
            }
        }
    }

    class Battlefild
    {
        private Squad _firstSquad;
        private Squad _secondSquad;

        public Battlefild()
        {
            _firstSquad = new Squad(new List<int> { 3, 2, 1, 4 });
            _secondSquad = new Squad(new List<int> { 2, 2, 3, 1 });
        }

        public void Battle()
        {
            while (_firstSquad.IsAlive == _secondSquad.IsAlive)
            {
                Console.WriteLine("Первый отряд атакует второй");
                _firstSquad.Attack(_secondSquad);
                Console.WriteLine("Второй отряд атакует первый");
                _secondSquad.Attack(_firstSquad);
                Console.WriteLine("Мертвых бойцов убрали с поля боя");
                _firstSquad.RemoveDeathSoldiers();
                _secondSquad.RemoveDeathSoldiers();
            }

            Result();
        }

        private void Result()
        {
            if (_firstSquad.IsAlive == _secondSquad.IsAlive)
            {
                Console.WriteLine("Отряды по убивали друг друга, никто не победил");
            }
            else if (_firstSquad.IsAlive == true & _secondSquad.IsAlive == false)
            {
                Console.WriteLine($"Победила Первый отряд");
            }
            else if (_secondSquad.IsAlive & _firstSquad.IsAlive == false)
            {
                Console.WriteLine($"Победила Второй отряд");
            }
        }
    }

    class Squad
    {
        public bool IsAlive = true;
        private List<Soldier> _soldiers;
        private List<Soldier> _typeSoldiers;

        public Squad(List<int> numberSoldiers)
        {
            _soldiers = new List<Soldier>();
            _typeSoldiers = new List<Soldier>()
            {
                new FirstTypeSoldier("Обычный солдат", 100 , 25, 10,1),
                new SecondTypeSoldier("Солдат атакует только одного", 100 , 25, 10,0),
                new ThirdTypeSoldier("Солдат атакует сразу нескольких", 100 , 25, 10,3),
                new FourthTypeSoldier("Солдат атакует сразу нескольких случайных,", 100 , 25, 10,3),
            };
            Create(numberSoldiers);
        }

        public void Attack(Squad squad)
        {
            for (int i = 0; i < _soldiers.Count; i++)
            {
                _soldiers[i].Attack(squad._soldiers);
            }
        }

        public void RemoveDeathSoldiers()
        {
            _soldiers.RemoveAll(soldier => soldier.Health == 0);

            if (_soldiers.Count == 0)
            {
                IsAlive = false;
            }
        }

        private void Create(List<int> numberSoldiers)
        {
            for (int i = 0; i < _typeSoldiers.Count; i++)
            {
                CreateSoldiers(numberSoldiers[i], i);
            }
        }

        private void CreateSoldiers(int numberSoldier, int idTypeSoldier)
        {
            for (int i = 0; i <= numberSoldier; i++)
            {
                _soldiers.Add(_typeSoldiers[idTypeSoldier].Clone());
            }
        }
    }

    abstract class Soldier
    {
        private int _modifierDefends = 2;

        public Soldier(string name, float health, float damage, float armor, int numberTarget)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
            QuantityTarget = numberTarget;
        }

        public string Name { get; protected set; }
        public float Health { get; protected set; }
        public float Damage { get; protected set; }
        public float Armor { get; protected set; }
        public int QuantityTarget { get; protected set; }

        public void TakeDamage(float damage)
        {
            float armor = damage / _modifierDefends;

            if (Armor <= armor)
            {
                Armor = 0;

                if (Health <= damage)
                {
                    damage = Health;
                }

                Health -= damage;
            }
            else
            {
                Armor -= armor;
            }
        }

        public abstract Soldier Clone();

        public virtual void Attack(List<Soldier> _target) { }
    }

    class Utils
    {
        private static Random _random = new Random();

        public Random GetRandom()
        {
            return _random;
        }
    }

    class FirstTypeSoldier : Soldier
    {
        private Utils _utils = new Utils();

        public FirstTypeSoldier(string name, float health, float damage, float armor, int quantityTarget) : base(name, health, damage, armor, quantityTarget) { }

        public override Soldier Clone()
        {
            return new FirstTypeSoldier(Name, Health, Damage, Armor, QuantityTarget);
        }

        public override void Attack(List<Soldier> _target)
        {
            Console.WriteLine("Aтакуют случайного солдат во вражеском взводе.");
            _target[_utils.GetRandom().Next(_target.Count)].TakeDamage(Damage);
        }
    }

    class SecondTypeSoldier : Soldier
    {
        private float _damageMultiplier;
        private float _finalDamage;

        public SecondTypeSoldier(string name, float health, float damage, float armor, int quantityTarget) : base(name, health, damage, armor, quantityTarget)
        {
            _damageMultiplier = 1.5f;
            _finalDamage = Damage * _damageMultiplier;
            QuantityTarget = 0;
        }

        public override Soldier Clone()
        {
            return new SecondTypeSoldier(Name, Health, Damage, Armor, QuantityTarget);
        }

        public override void Attack(List<Soldier> _target)
        {
            Console.WriteLine("Aтакует только одного, но с множителем урона.");
            _target[QuantityTarget].TakeDamage(_finalDamage);
        }
    }

    class ThirdTypeSoldier : Soldier
    {
        public ThirdTypeSoldier(string name, float health, float damage, float armor, int quantityTarget) : base(name, health, damage, armor, quantityTarget) { }

        public override Soldier Clone()
        {
            return new ThirdTypeSoldier(Name, Health, Damage, Armor, QuantityTarget);
        }

        public override void Attack(List<Soldier> _targets)
        {
            Console.WriteLine("Aтакует сразу нескольких, без повторения атакованного за свою атаку.");

            for (int i = 0; i < QuantityTarget; i++)
            {
                _targets[i].TakeDamage(Damage);
            }
        }
    }

    class FourthTypeSoldier : Soldier
    {
        private Utils _utils = new Utils();

        public FourthTypeSoldier(string name, float health, float damage, float armor, int quantityTarget) : base(name, health, damage, armor, quantityTarget) { }

        public override Soldier Clone()
        {
            return new FourthTypeSoldier(Name, Health, Damage, Armor, QuantityTarget);
        }

        public override void Attack(List<Soldier> _targets)
        {
            Console.WriteLine("Aтакует сразу нескольких, атакованные солдаты могут повторяться");

            for (int i = 0; i <= QuantityTarget; i++)
            {
                _targets[_utils.GetRandom().Next(_targets.Count)].TakeDamage(Damage);
            }
        }
    }
}