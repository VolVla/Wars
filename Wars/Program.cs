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
            _firstSquad.ShowUnits(_firstSquad.UnitSquad);
            Console.WriteLine($"\n{_secondSquad.NameSquad}");
            _secondSquad.ShowUnits(_secondSquad.UnitSquad);
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
            if (_firstSquad.UnitSquad.Count == 0)
            {
                Console.WriteLine($"Победил {_secondSquad.NameSquad} отряд !");
            }
            else if (_secondSquad.UnitSquad.Count == 0)
            {
                Console.WriteLine($"Победил {_firstSquad.NameSquad} отряд !");
            }
            else if (_firstSquad.UnitSquad.Count == 0 && _secondSquad.UnitSquad.Count == 0)
            {
                Console.WriteLine("Поздравляю отряды двух стран убили друг друга, никто не победил и все проиграли");
            }
        }

        private void AttackSquad()
        {
            List<CombatUnits> firstSquad = _firstSquad.UnitSquad;
            List<CombatUnits> secondSquad = _secondSquad.UnitSquad;

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
        protected const string SNIPER = "Снайпер";
        protected const string TANK = "Танк";
        protected const string ARTELERIST = "Артилерист";
        protected const string ARNOREDVEHICLE = "БТР";
        protected const string INFANTRY = "Пехотинец";
        protected const string SAPPER = "Сапер";
        public bool IsLiveSquad;
        public List<CombatUnits> UnitSquad;
        private bool _isCorrectCreateSquad;
        private ConsoleKey _key;
        private List<CombatUnits> _listCombatUnits;

        public Squad()
        {
            IsLiveSquad = true;
            UnitSquad = new List<CombatUnits>();
            _listCombatUnits = new List<CombatUnits>();
            _isCorrectCreateSquad = false;
            _key = ConsoleKey.F;
            _listCombatUnits.Add(new ArmoredVehicle(ARNOREDVEHICLE, 160, 60, 120, 5, true, 2,false,0));
            _listCombatUnits.Add(new Infantry(INFANTRY, 50, 30, 50, 5, true, 2, false, 0));
            _listCombatUnits.Add(new Sapper(SAPPER, 60, 20, 40, 5, true, 1, false, 0));
            _listCombatUnits.Add(new Sniper(SNIPER, 40, 100, 10, 5, true, 4, false, 0));
            _listCombatUnits.Add(new Tank(TANK, 200, 80, 200, 5, true, 3, false, 0));
            _listCombatUnits.Add(new Artelerist(ARTELERIST, 100, 200, 10, 5, true, 5, false, 0));
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
                    UnitSquad.Add(_listCombatUnits[inputID - 1].Clone());
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

    abstract class CombatUnits : Squad
    {
        public CombatUnits(string name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit,bool isStand, int standCoin)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
            Distance = distance;
            IsLive = isLive;
            DistanceAttackUnit = distanceAttackUnit;
            IsStand = isStand;
            TimeStandPoint = standCoin;
        }

        public bool IsStand { get; private set; }
        public bool IsLive { get; protected set; }
        public int TimeStandPoint { get; protected set; }
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

        public void DebafResistence(float _debafArmorResistence)
        {
            int minimumDamageResistence = 0;

            if (DamageResistence > minimumDamageResistence)
            {
                DamageResistence -= _debafArmorResistence;
            }
        }

        public void UpTimeStandPoint(int timepoint)
        {
            TimeStandPoint += timepoint;
        }

        public void UpAttackDistance(int distanceEnemy)
        {
            int maximumDistanceAttackUnit = 5;

            if (DistanceAttackUnit <= maximumDistanceAttackUnit)
            {
                DistanceAttackUnit = distanceEnemy;
            }
        }

        public void UpDamageAttack(float bonusDamage)
        {
            Damage = +bonusDamage;
        }

        public void MoveUnit()
        {
            if (TimeStandPoint <= 0)
            {
                if (Distance > 0)
                {
                    Distance--;
                }
            }
            else if (TimeStandPoint > 0) 
            {
                TimeStandPoint--;
            }
        }

        public void KillUnit()
        {
            Console.WriteLine("Вас попали смертельным снарядом");
            IsLive = false;
        }

        public bool SuccessfukApplicationSkillAttack()
        {
            bool isSuccessful = false;
            int minimum = 0; 
            int maximum = 2;
            int number;
            Random random = new Random();
            number = random.Next(minimum, maximum);

            if (number == minimum) 
            {
                isSuccessful = false;
            }
            else if (number == maximum)
            {
                isSuccessful = true;
            }

            return isSuccessful;
        }

        public abstract CombatUnits Clone();

        protected virtual void Attack(CombatUnits unit) { }

        private void CheckDeath()
        {
            if (Health <= 0)
            {
                Distance--;
            }
        }
    }

    class Sniper : CombatUnits
    {
        private int _debaffArmor = 10;
        private int _timeStandEnemyUnit = 1;

        public Sniper(string name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit, bool isStand,int timeStandPoint) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit, isStand, timeStandPoint) { }

        public override CombatUnits Clone()
        {
            return new Sniper(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit, IsStand, TimeStandPoint);
        }

        protected override void Attack(CombatUnits unit)
        {
            switch (unit.Name)
            {
                case SNIPER:
                    if(SuccessfukApplicationSkillAttack()) 
                       SpellAttackRangeUnit(unit);
                    break;
                case TANK:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                case INFANTRY:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                default:
                    unit.CauseDamage(this);
                    break;
            }
        }

        private void SpellAttackMeleUnit(CombatUnits unit)
        {
            unit.UpTimeStandPoint(_timeStandEnemyUnit);
        }

        private void SpellAttackRangeUnit(CombatUnits unit)
        {
            unit.DebafResistence(_debaffArmor);
        }
    }

    class Tank : CombatUnits
    {
        private float _bonusDamage = 20;
        private float _baseDamage;
        private int _timeUseSkill = 2 ;
        private int _temporaryCoinUseSkill = 0;

        public Tank(string name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit, bool isStand, int timeStandPoint) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit,isStand, timeStandPoint) 
        {
            _baseDamage = Damage;
        }

        public override CombatUnits Clone()
        {
            return new Tank(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit,IsStand,TimeStandPoint);
        }

        protected override void Attack(CombatUnits unit)
        {
            switch (unit.Name)
            {
                case SAPPER:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit();
                    break;
                case ARTELERIST:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackRangeUnit();
                    break;
                case ARNOREDVEHICLE:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit();
                    break;
                default:
                    MoveUnit();
                    break;
            }
        }

        private void SpellAttackMeleUnit()
        {
            if (_timeUseSkill == _temporaryCoinUseSkill)
            {
                Console.WriteLine("Использовал способной растрел увеличи урон на эту атаку");
                Damage += _bonusDamage;
                _temporaryCoinUseSkill = 0;
            }
            else
            {
                Damage = _baseDamage;
                _temporaryCoinUseSkill++;
            } 
        }

        private void SpellAttackRangeUnit()
        {
            Console.WriteLine("Марш Бросок");
            MoveUnit();
        }
    }

    class Artelerist : CombatUnits
    {
        private int _chanceHit = 0;
        private int _missShoot = 0;
        private int _hitShootEnemy = 2;
        private Random random = new Random();

        public Artelerist(string name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit, bool isStand, int timeStandPoint) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit, isStand, timeStandPoint) { }

        public override CombatUnits Clone()
        {
            return new Artelerist(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit,IsStand,TimeStandPoint);
        }

        protected override void Attack(CombatUnits unit)
        {
            switch (unit.Name)
            {
                case SAPPER:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                case INFANTRY:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                case TANK:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                default:
                    unit.CauseDamage(this);
                    break;
            }
        }

        private void SpellAttackMeleUnit(CombatUnits unit)
        {
            _chanceHit = random.Next(_missShoot, _hitShootEnemy);

            if (_chanceHit == _hitShootEnemy)
            {
                unit.KillUnit();
            }
        }
    }

    class Infantry : CombatUnits
    {
        private int _upDistanceAttack = 3;
        private int _coinUpDistanceAttack = 0;
        private int _timeStandPoint = 2;

        public Infantry(string name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit, bool isStand, int timeStandPoint) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit,isStand, timeStandPoint) {}

        public override CombatUnits Clone()
        {
            return new Infantry(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit,IsStand,TimeStandPoint);
        }

        protected override void Attack(CombatUnits unit)
        {
            switch (unit.Name)
            {
                case SAPPER:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                case ARTELERIST:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackRangeUnit(unit);
                    break;
                case ARNOREDVEHICLE:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                default:
                    MoveUnit();
                    break;
            }
        }

        private void SpellAttackMeleUnit(CombatUnits unit)
        {
            if (_coinUpDistanceAttack == _upDistanceAttack)
            {
                Console.WriteLine("За родину,Бойцы воодушивились и на целились на врага дистанция для атаки увеличилась");
                UpAttackDistance(unit.Distance);
                _coinUpDistanceAttack = 0;
            }

            _coinUpDistanceAttack++;
        }

        private void SpellAttackRangeUnit(CombatUnits unit)
        {
            Console.WriteLine("Кидает гранату оглушает противника");
            unit.UpTimeStandPoint(_timeStandPoint);
        }
    }

    class ArmoredVehicle : CombatUnits
    {
        private int _amountOfCoinUseSkill = 3;
        private float _buffArmor = 10;
        private int _minimumCoinUseSkill = 0;

        public ArmoredVehicle(string name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit, bool isStand,int timeStandPoint) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit,isStand, timeStandPoint) { }

        public override CombatUnits Clone()
        {
            return new ArmoredVehicle(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit,IsStand, TimeStandPoint);
        }

        protected override void Attack(CombatUnits unit)
        {
            switch (unit.Name)
            {
                case SAPPER:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit();
                    break;
                case INFANTRY:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit();
                    break;
                case TANK:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit();
                    break;
                default:
                    MoveUnit();
                    break;
            }
        }

        private void SpellAttackMeleUnit()
        {
            if(_amountOfCoinUseSkill > _minimumCoinUseSkill)
            {
                Console.WriteLine("Скорость наше все пока мы едем броня увеличена");
                MoveUnit();
                Armor += _buffArmor;
            }
        }
    }

    class Sapper : CombatUnits
    {
        private float _bonusDamageAttack = 20;
        private float _debafResistence = 10;

        public Sapper(string name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit, bool isStand,int timeStandPoint) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit,isStand, timeStandPoint) { }

        public override CombatUnits Clone()
        {
            return new Sapper(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit,IsStand, TimeStandPoint);
        }

        protected override void Attack(CombatUnits unit)
        {
            switch (unit.Name)
            {
                case ARNOREDVEHICLE:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                case INFANTRY:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                case TANK:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                default:
                    MoveUnit();
                    break;
            }
        }

        private void SpellAttackMeleUnit(CombatUnits unit)
        {
            if(Distance == unit.DistanceAttackUnit)
            {
                Console.WriteLine("Враг на дистанции атаки, достаем гранатомёт и атакуем.Сопротивление врага уменьшилась, а наносимый урон увеличился");
                unit.DebafResistence(_debafResistence);
                UpDamageAttack(_bonusDamageAttack);
            }
        }
    }
}