using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.ComponentModel;
using System.Collections;

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

                if (true)
                {
                    Console.WriteLine("Для начало сражения нажмите на любую клавишу");
                    Console.ReadKey();
                    Console.Clear();
                    battleArena.Battle();
                    battleArena.AnnounceWinner();
                }

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
        Squad _firstSquad;
        Squad _secondSquad;

        public BattelField()
        {
            _firstSquad = new Squad();
            _secondSquad = new Squad();
        }

        public void TryCreativeBattle()
        {
            _firstSquad.CreativeSquad();
            _secondSquad.CreativeSquad();
        }

        public void Battle()
        {
            while (_firstSquad.CheckIsLiveSquad() == true && _secondSquad.CheckIsLiveSquad() == true)
            {
                AttackSquad();
                AnnounceWinner();
            }
        }

        public void AnnounceWinner()
        {
            if (_firstSquad.CheckIsLiveSquad() == false)
            {
                Console.WriteLine($"Победил {_secondSquad.NameSquad} !");
            }
            else if (_secondSquad.CheckIsLiveSquad() == false)
            {
                Console.WriteLine($"Победил {_firstSquad.NameSquad} !");
            }
            else if (_firstSquad.CheckIsLiveSquad() == false && _secondSquad.CheckIsLiveSquad() == false)
            {
                Console.WriteLine("Поздравляю бойцы убили друг друга, никто не победил и все проиграли");
            }
        }

        public void AttackSquad()
        {
            List<CombatUnit> first = _firstSquad.ReturnListSquad();
            List<CombatUnit> second = _secondSquad.ReturnListSquad();

            for (int i = 0; i < second.Count; i++)
            {
                if (first[i].Initiative > second[i].Initiative)
                {
                    if (first[i].Distance >= second[i].Distance)
                    {
                        second[i].CauseDamage(first[i]);
                    }
                    else
                    {
                        first[i].MoveUnit();
                    }
                }
                else if (first[i].Initiative < second[i].Initiative)
                {

                }
                else if (first[i].Initiative == second[i].Initiative)
                {

                }


            }
        }
    }

    class Squad
    {
        public bool _isLiveSquad = true;
        private List<CombatUnit> _squad = new List<CombatUnit>();
        private CombatUnit _units;
        private bool _isCorrectCreatSquad = false;
        private ConsoleKey _key = ConsoleKey.F;

        public string NameSquad { get; private set; }

        public List<CombatUnit> CreativeSquad()
        {
            while (_isCorrectCreatSquad == false)
            {
                Console.WriteLine("Перед созданием отряда придумайте ему пафосное название");
                NameSquad = Console.ReadLine();
                Console.WriteLine("Добро пожаловать в создание отряда, выберете бойца который пресоединиться к вашему отряду");
                _units.ShowUnits();
                bool _check = int.TryParse(Console.ReadLine(), out int result);
                _squad.Add(_units.Clone());
                Console.WriteLine($"Что бы закончить выбор  бойцов нажмите на {_key}, иначе любую кнопку");

                if (Console.ReadKey().Key == _key)
                {
                    _isCorrectCreatSquad = true;
                }
            }

            return _squad;
        }

        public bool CheckIsLiveSquad()
        {
            int _coinDeatch = 0;

            for (int i = 0; i < _squad.Count; i++)
            {
                if (_squad[i].IsLive == false)
                {
                    _coinDeatch++;
                }
            }

            if (_squad.Count == _coinDeatch)
            {
                _isLiveSquad = false;
            }

            return _isLiveSquad;
        }

        public int CheckInitiative()
        {
            return _units.Initiative;
        }

        public List<CombatUnit> ReturnListSquad()
        {
            return _squad;
        }
    }

    abstract class CombatUnit
    {
        protected const string SNIPER = "Снайпер";
        protected const string TANK = "Танк";
        protected const string ARTELERIST = "Артилерист";
        protected const string ARNOREDVEHICLE = "Снайпер";
        protected const string INFANTRY = "Солдат";
        protected const string SAPPER = "Артилерист";
        protected List<CombatUnit> _squad = new List<CombatUnit>();

        public CombatUnit(string name, float health, float damage, float armor, int distance, int initiative, bool isLive)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
            Distance = distance;
            Initiative = initiative;
            IsLive = isLive;
        }

        public int Distance { get; private set; }
        public string Name { get; protected set; }
        public float Health { get; protected set; }
        public float Armor { get; protected set; }
        public float Damage { get; protected set; }
        public int Initiative { get; protected set; }
        public bool IsLive { get; protected set; }
        public float DamageResistence { get; protected set; }

        public void ShowUnits()
        {
            for (int i = 0; i < _squad.Count; i++)
            {
                Console.WriteLine($"Имя юнита - {_squad[i].Name}, здоровье юнита - {_squad[i].Health}, броня юнита - {_squad[i].Armor}, наносимый урон - {_squad[i].Damage},необходимое расстояние для атаки - {_squad[i].Distance}, показатель кто первый атакует {_squad[i].Initiative}");
            }
        }

        public void CauseDamage(CombatUnit unit)
        {
            Attack(this);
            if (Armor == 0)
            {
                Health -= unit.Damage;

            }
            else
            {

                Armor = unit.Damage - (Armor * DamageResistence);
            }
        }
        public void DamageResisten(float _debafArmorResistence)
        {
            if (DamageResistence > 0)
            {
                DamageResistence -= _debafArmorResistence;
            }
        }
        public void CheckDeath()
        {
            if (Health < 0)
            {
                IsLive = false;
            }
        }

        public void MoveUnit()
        {
            Distance--;
        }

        public abstract CombatUnit Clone();
        public virtual void Attack(CombatUnit unit) { }
    }

    abstract class MeleDamageUnit : CombatUnit
    {

        public MeleDamageUnit(string name, float health, float damage, float armor, int distance, int initiative, bool isLive) : base(name, health, damage, armor, distance, initiative, isLive)
        {
            _squad.Add(new ArmoredVehicle(ARNOREDVEHICLE, 100, 100, 20, 1, 1, true));
            _squad.Add(new Infantry(INFANTRY, 100, 100, 20, 2, 2, true));
            _squad.Add(new Sapper(SAPPER, 200, 49, 40, 2, 1, true));
        }
    }

    abstract class RangeDamageUnit : CombatUnit
    {
        public RangeDamageUnit(string name, float health, float damage, float armor, int distance, int initiative, bool isLive) : base(name, health, damage, armor, distance, initiative, isLive)
        {
            _squad.Add(new Sniper(SNIPER, 100, 100, 20, 5, 2, true));
            _squad.Add(new Tank(TANK, 100, 100, 20, 3, 2, true));
            _squad.Add(new Artelerist(ARTELERIST, 100, 100, 20, 4, 2, true));
        }
    }

    class Sniper : RangeDamageUnit
    {
        public Sniper(string name, float health, float damage, float armor, int distance, int initiative, bool isLive) : base(name, health, damage, armor, distance, initiative, isLive) { }
        public override CombatUnit Clone()
        {
            return new Sniper(Name, Health, Damage, Armor, Distance, Initiative, IsLive);
        }
    }

    class Tank : RangeDamageUnit
    {
        public Tank(string name, float health, float damage, float armor, int distance, int initiative, bool isLive) : base(name, health, damage, armor, distance, initiative, isLive) { }
        public override CombatUnit Clone()
        {
            return new Tank(Name, Health, Damage, Armor, Distance, Initiative, IsLive);
        }
    }

    class Artelerist : RangeDamageUnit
    {
        private float _bonusDamage = 2;
        private float _debafArmorResistence = 20;
        public Artelerist(string name, float health, float damage, float armor, int distance, int initiative, bool isLive) : base(name, health, damage, armor, distance, initiative, isLive) { }

        public override void Attack(CombatUnit unit)
        {

            switch (unit.Name)
            {
                case SNIPER:
                    Damage = Damage * _bonusDamage;
                    break;
                case TANK:
                    unit.DamageResisten(_debafArmorResistence);
                    break;
                case INFANTRY:
                    break;
            }






        }

        public override CombatUnit Clone()
        {
            return new Artelerist(Name, Health, Damage, Armor, Distance, Initiative, IsLive);
        }

    }

    class Infantry : MeleDamageUnit
    {
        public Infantry(string name, float health, float damage, float armor, int distance, int initiative, bool isLive) : base(name, health, damage, armor, distance, initiative, isLive) { }
        public override CombatUnit Clone()
        {
            return new Infantry(Name, Health, Damage, Armor, Distance, Initiative, IsLive);
        }
    }

    class ArmoredVehicle : MeleDamageUnit
    {
        public ArmoredVehicle(string name, float health, float damage, float armor, int distance, int initiative, bool isLive) : base(name, health, damage, armor, distance, initiative, isLive) { }

        public override CombatUnit Clone()
        {
            return new ArmoredVehicle(Name, Health, Damage, Armor, Distance, Initiative, IsLive);
        }

    }

    class Sapper : MeleDamageUnit
    {
        public Sapper(string name, float health, float damage, float armor, int distance, int initiative, bool isLive) : base(name, health, damage, armor, distance, initiative, isLive) { }
        public override CombatUnit Clone()
        {
            return new Sapper(Name, Health, Damage, Armor, Distance, Initiative, IsLive);
        }
    }
}
