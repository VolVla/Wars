using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.ComponentModel;
using System.Collections;
using System.Security.Cryptography;

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
                BattelField battleArena = new BattelField();
                Console.WriteLine("Для начало сражения нажмите на любую клавишу");
                Console.ReadKey();
                Console.Clear();
                battleArena.TryCreativeBattle();
                battleArena.Battle();
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

    class BattelField
    {
        private Squad _firstSquad;
        private Squad _secondSquad;

        public BattelField()
        {
            _firstSquad = new Squad();
            _secondSquad = new Squad();
        }

        public void TryCreativeBattle()
        {
            _firstSquad.CreativeSquad();
            _secondSquad.CreativeSquad();
            Console.WriteLine(_firstSquad.NameSquad);
            _firstSquad.ShowUnits(_firstSquad.squad);
            Console.WriteLine($"\n{_secondSquad.NameSquad}");
            _secondSquad.ShowUnits(_secondSquad.squad);
            Console.WriteLine("\nДля начала сражение нажмите любую кнопку");
            Console.ReadKey();
            Console.Clear();
        }

        public void Battle()
        {
            AttackSquad();
            AnnounceWinner();
        }

        private void AnnounceWinner()
        {
            if (_firstSquad.squad.Count == 0)
            {
                Console.WriteLine($"Победил {_secondSquad.NameSquad} отряд !");
            }
            else if (_secondSquad.squad.Count == 0)
            {
                Console.WriteLine($"Победил {_firstSquad.NameSquad} отряд !");
            }
            else if (_firstSquad.squad.Count == 0 && _secondSquad.squad.Count == 0)
            {
                Console.WriteLine("Поздравляю отряды двух стран убили друг друга, никто не победил и все проиграли");
            }
        }

        private void AttackSquad()
        {
            List<CombatUnits> firstSquad = _firstSquad.squad;
            List<CombatUnits> secondSquad = _secondSquad.squad;

            while (firstSquad.Count > 0 && secondSquad.Count > 0)
            {
                BattleSquads(firstSquad, secondSquad);
                BattleSquads(secondSquad, firstSquad);
            }
        }

        private void BattleSquads(List<CombatUnits> _attackSquad, List<CombatUnits> _defendSquad)
        {
            int firstDefender = 0;

            for (int i = 0; i < _attackSquad.Count; i++)
            {
                if (_attackSquad.Count >= _defendSquad.Count)
                {
                    if (i >= _defendSquad.Count)
                    {
                        CheckDistanceAttackUnit(_attackSquad, _defendSquad, i, firstDefender);
                    }
                    else
                    {
                        CheckDistanceAttackUnit(_attackSquad, _defendSquad, i, i);
                    }
                }
                else if (_attackSquad.Count < _defendSquad.Count)
                {
                    CheckDistanceAttackUnit(_attackSquad, _defendSquad, i, i);
                }
            }
        }

        private void CheckDistanceAttackUnit(List<CombatUnits> _attackSquad, List<CombatUnits> _defendSquad, int idUnit, int temporary)
        {
            if (_defendSquad.Count != 0)
            {
                if (_attackSquad[idUnit].DistanceAttackUnit >= _defendSquad[temporary].Distance)
                {
                    _defendSquad[temporary].CauseDamage(_attackSquad[idUnit]);

                    if (_defendSquad[temporary].IsLive == false)
                    {
                        _defendSquad.RemoveAt(temporary);
                    }
                }
                else
                {
                    _attackSquad[idUnit].MoveUnit();
                }
            }
        }
    }

    class Squad
    {
        public bool isLiveSquad = true;
        public List<CombatUnits> squad;
        private const string SNIPER = "Снайпер";
        private const string TANK = "Танк";
        private const string ARTELERIST = "Артилерист";
        private const string ARNOREDVEHICLE = "БТР";
        private const string INFANTRY = "Пехотинец";
        private const string SAPPER = "Сапер";
        private bool _isCorrectCreateSquad;
        private ConsoleKey _key;
        private List<CombatUnits> _listCombatUnits;

        public Squad()
        {
            squad = new List<CombatUnits>();
            _listCombatUnits = new List<CombatUnits>();
            _isCorrectCreateSquad = false;
            _key = ConsoleKey.F;
            _listCombatUnits.Add(new ArmoredVehicle(ARNOREDVEHICLE, 160, 60, 120, 5, true, 2));
            _listCombatUnits.Add(new Infantry(INFANTRY, 50, 30, 50, 5, true, 2));
            _listCombatUnits.Add(new Sapper(SAPPER, 60, 20, 40, 5, true, 1));
            _listCombatUnits.Add(new Sniper(SNIPER, 40, 100, 10, 5, true, 4));
            _listCombatUnits.Add(new Tank(TANK, 200, 80, 200, 5, true, 3));
            _listCombatUnits.Add(new Artelerist(ARTELERIST, 100, 200, 10, 5, true, 5));
        }

        public string NameSquad { get; private set; }

        public void ShowUnits(List<CombatUnits> _unitsSquad)
        {
            for (int i = 0; i < _unitsSquad.Count; i++)
            {
                Console.WriteLine($"Номер юнита - {i + 1},Имя юнита - {_unitsSquad[i].Name}, здоровье юнита - {_unitsSquad[i].Health}, броня юнита - {_unitsSquad[i].Armor}, наносимый урон - {_unitsSquad[i].Damage}\n,необходимое расстояние для атаки - {_unitsSquad[i].DistanceAttackUnit}");
            }
        }

        public void CreativeSquad()
        {
            Console.WriteLine("Перед созданием отряда придумайте ему пафосное название");
            NameSquad = Console.ReadLine();

            while (_isCorrectCreateSquad == false)
            {
                Console.WriteLine("Добро пожаловать в создание отряда, выберете бойца который пресоединиться к вашему отряду");
                ShowUnits(_listCombatUnits);
                int.TryParse(Console.ReadLine(), out int inputID);

                if (inputID <= 0 || inputID - 1 > _listCombatUnits.Count)
                {
                    Console.WriteLine("Вы ввели не коректные данные.");
                }
                else if (inputID > 0 && inputID - 1 < _listCombatUnits.Count)
                {
                    Console.WriteLine("Солдат успешно выбран.");
                    squad.Add(_listCombatUnits[inputID - 1].Clone());
                }

                Console.WriteLine($"Что бы закончить выбор  бойцов нажмите на {_key}, иначе любую кнопку");

                if (Console.ReadKey().Key == _key)
                {
                    _isCorrectCreateSquad = true;
                }

                Console.Clear();
            }

            Console.Clear();
        }
    }

    abstract class CombatUnits
    {
        public CombatUnits(string name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
            Distance = distance;
            IsLive = isLive;
            DistanceAttackUnit = distanceAttackUnit;
        }

        public bool IsLive { get; protected set; }
        public int DistanceAttackUnit { get; private set; }
        public int Distance { get; private set; }
        public float Health { get; protected set; }
        public float Armor { get; protected set; }
        public float Damage { get; protected set; }
        public float DamageResistence { get; protected set; }
        public string Name { get; protected set; }

        public void CauseDamage(CombatUnits unit)
        {
            float finalDamage = 0;
            unit.Attack(this);

            if (Armor <= 0)
            {
                finalDamage = unit.Damage;
                Health -= finalDamage;
            }
            else if (unit.Damage >= (Armor * DamageResistence))
            {
                finalDamage = unit.Damage - (Armor * DamageResistence);
                Armor = 0;
                Health -= finalDamage;
            }
            else if (unit.Damage < (Armor * DamageResistence))
            {
                finalDamage = unit.Damage - (Armor * DamageResistence);
                Armor -= finalDamage;
            }

            Console.WriteLine($"Юнит {unit.Name} нанес  урон  бойцу {Name} в размере {finalDamage}, у него осталось - {Health} здоровья и {Armor} брони");
            CheckDeath();
        }

        public void DamageResistens(float _debafArmorResistence)
        {
            if (DamageResistence > 0)
            {
                DamageResistence -= _debafArmorResistence;
            }
        }

        public void MoveUnit()
        {
            if (Distance > 0)
            {
                Distance--;
            }
        }

        public abstract CombatUnits Clone();

        protected virtual void Attack(CombatUnits unit) { }

        private void CheckDeath()
        {
            if (Health <= 0)
            {
                IsLive = false;
            }
        }
    }

    class Sniper : CombatUnits
    {
        public Sniper(string name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit) { }

        public override CombatUnits Clone()
        {
            return new Sniper(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit);
        }

        protected override void Attack(CombatUnits unit)
        {
            //switch (unit.Name)
            //{
            //    case SNIPER:
            //        break;
            //    case TANK:
            //        unit.DamageResisten(_debafArmorResistence);
            //        break;
            //    case INFANTRY:
            //        Damage = Damage * _bonusDamage;
            //        break;
            //}
        }
    }

    class Tank : CombatUnits
    {
        public Tank(string name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit) { }

        public override CombatUnits Clone()
        {
            return new Tank(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit);
        }

        protected override void Attack(CombatUnits unit)
        {
            //switch (unit.Name)
            //{
            //    case SNIPER:
            //        break;
            //    case TANK:
            //        unit.DamageResisten(_debafArmorResistence);
            //        break;
            //    case INFANTRY:
            //        Damage = Damage * _bonusDamage;
            //        break;
            //}
        }
    }

    class Artelerist : CombatUnits
    {
        private float _bonusDamage = 1.5f;
        private float _debafArmorResistence = 20;
        public Artelerist(string name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit) { }

        protected override void Attack(CombatUnits unit)
        {
            //switch (unit.Name)
            //{
            //    case SNIPER:
            //        break;
            //    case TANK:
            //        unit.DamageResisten(_debafArmorResistence);
            //        break;
            //    case INFANTRY:
            //        Damage = Damage * _bonusDamage;
            //        break;
            //}
        }

        public override CombatUnits Clone()
        {
            return new Artelerist(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit);
        }
    }

    class Infantry : CombatUnits
    {
        public Infantry(string name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit) { }

        public override CombatUnits Clone()
        {
            return new Infantry(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit);
        }

        protected override void Attack(CombatUnits unit)
        {
            //switch (unit.Name)
            //{
            //    case SNIPER:
            //        break;
            //    case TANK:
            //        unit.DamageResisten(_debafArmorResistence);
            //        break;
            //    case INFANTRY:
            //        Damage = Damage * _bonusDamage;
            //        break;
            //}
        }
    }

    class ArmoredVehicle : CombatUnits
    {
        public ArmoredVehicle(string name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit) { }

        public override CombatUnits Clone()
        {
            return new ArmoredVehicle(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit);
        }

        protected override void Attack(CombatUnits unit)
        {
            //switch (unit.Name)
            //{
            //    case SNIPER:
            //        break;
            //    case TANK:
            //        unit.DamageResisten(_debafArmorResistence);
            //        break;
            //    case INFANTRY:
            //        Damage = Damage * _bonusDamage;
            //        break;
            //}
        }
    }

    class Sapper : CombatUnits
    {
        public Sapper(string name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit) { }

        public override CombatUnits Clone()
        {
            return new Sapper(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit);
        }

        protected override void Attack(CombatUnits unit)
        {
            //switch (unit.Name)
            //{
            //    case SNIPER:
            //        break;
            //    case TANK:
            //        unit.DamageResisten(_debafArmorResistence);
            //        break;
            //    case INFANTRY:
            //        Damage = Damage * _bonusDamage;
            //        break;
            //}
        }
    }
}