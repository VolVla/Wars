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
        private List<Squad> _squads;
        private int _firstSquad = 0;
        private int _secondSquad = 1;

        public Battlefild()
        {
            _squads = new List<Squad>()
            {
                new Squad(3,2,2,4),
                new Squad(2,2,3,1)
            };
        }

        public void Battle()
        {
            while (_squads[_firstSquad].IsAlive == _squads[_secondSquad].IsAlive)
            {
                Console.WriteLine("Первый отряд атакует второй");
                _squads[_firstSquad].Attack(_squads[_secondSquad]);
                Console.WriteLine("Второй отряд атакует первый");
                _squads[_secondSquad].Attack(_squads[_firstSquad]);
                Console.WriteLine("Мертвых бойцов убрали с поля боя");
                _squads[_firstSquad].RemoveDeathSoldiers();
                _squads[_secondSquad].RemoveDeathSoldiers();
            }

            Result();
        }

        private void Result()
        {
            if (_squads[_firstSquad].IsAlive == _squads[_secondSquad].IsAlive)
            {
                Console.WriteLine("Отряды по убивали друг друга, никто не победил");
            }
            else if (_squads[_firstSquad].IsAlive == true & _squads[_secondSquad].IsAlive == false)
            {
                Console.WriteLine($"Победила Первый отряд");
            }
            else if (_squads[_secondSquad].IsAlive & _squads[_firstSquad].IsAlive == false)
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

        public Squad(int numberSoldiersTypeFirst, int numberSoldiersTypeSecond, int numberSoldiersTypeThird, int numberSoldiersTypeFourth)
        {
            _typeSoldiers = new List<Soldier>()
            {
                new FirstTypeSoldier("Обычный солдат", 100 , 25, 10,1),
                new SecondTypeSoldier("Солдат атакует только одного", 100 , 25, 10,0),
                new ThirdTypeSoldier("Солдат атакует сразу нескольких", 100 , 25, 10,3),
                new FourthTypeSoldier("Солдат атакует сразу нескольких случайных,", 100 , 25, 10,3),
            };
            CreateSoldiers(numberSoldiersTypeFirst, numberSoldiersTypeSecond, numberSoldiersTypeThird, numberSoldiersTypeFourth);
        }

        public void Attack(Squad squad)
        {
            for (int i = 0; i <= _soldiers.Count; i++)
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

        private void CreateSoldiers(int numberSoldiersTypeFirst, int numberSoldiersTypeSecond, int numberSoldiersTypeThird, int numberSoldiersTypeFourth)
        {
            CreateSoldier(numberSoldiersTypeFirst, _typeSoldiers[0]);
            CreateSoldier(numberSoldiersTypeSecond, _typeSoldiers[1]);
            CreateSoldier(numberSoldiersTypeThird, _typeSoldiers[2]);
            CreateSoldier(numberSoldiersTypeFourth, _typeSoldiers[3]);
        }

        private void CreateSoldier(int numberSoldier, Soldier soldier)
        {
            for (int i = 0; i < numberSoldier; i++)
            {
                _soldiers.Add(soldier.Clone());
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

    class FirstTypeSoldier : Soldier
    {
        private Random _random;

        public FirstTypeSoldier(string name, float health, float damage, float armor, int numberTarget) : base(name, health, damage, armor, numberTarget) { }

        public override Soldier Clone()
        {
            return new FirstTypeSoldier(Name, Health, Damage, Armor, QuantityTarget);
        }

        public override void Attack(List<Soldier> _target)
        {
            _target[_random.Next(_target.Count)].TakeDamage(Damage);
        }
    }

    class SecondTypeSoldier : Soldier
    {
        private float _damageMultiplier;
        private float _finalDamage;

        public SecondTypeSoldier(string name, float health, float damage, float armor, int numberTarget) : base(name, health, damage, armor, numberTarget)
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
            _target[QuantityTarget].TakeDamage(_finalDamage);
        }
    }

    class ThirdTypeSoldier : Soldier
    {
        public ThirdTypeSoldier(string name, float health, float damage, float armor, int numberTarget) : base(name, health, damage, armor, numberTarget) { }

        public override Soldier Clone()
        {
            return new ThirdTypeSoldier(Name, Health, Damage, Armor, QuantityTarget);
        }

        public override void Attack(List<Soldier> _targets)
        {
            for (int i = 0; i <= QuantityTarget; i++)
            {
                _targets[i].TakeDamage(Damage);
            }
        }
    }

    class FourthTypeSoldier : Soldier
    {
        private Random _random;

        public FourthTypeSoldier(string name, float health, float damage, float armor, int numberTarget) : base(name, health, damage, armor, numberTarget) { }

        public override Soldier Clone()
        {
            return new FourthTypeSoldier(Name, Health, Damage, Armor, QuantityTarget);
        }

        public override void Attack(List<Soldier> _targets)
        {
            for (int i = 0; i <= QuantityTarget; i++)
            {
                _targets[_random.Next(_targets.Count)].TakeDamage(Damage);
            }
        }
    }
}