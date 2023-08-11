using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                battleArena.CreativeBattle();
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

    enum NameUnits
    {
        Снайпер,
        Танк,
        Артилерист,
        БТР,
        Пехотинец,
        Сапер
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

        public void CreativeBattle()
        {
            _firstSquad.RecruitmentUnit();
            _secondSquad.RecruitmentUnit();
            Console.WriteLine(_firstSquad.Name);
            _firstSquad.ShowUnits(_firstSquad.Units);
            Console.WriteLine($"\n{_secondSquad.Name}");
            _secondSquad.ShowUnits(_secondSquad.Units);
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
            if (_firstSquad.Units.Count == 0)
            {
                Console.WriteLine($"Победил {_secondSquad.Name} отряд !");
            }
            else if (_secondSquad.Units.Count == 0)
            {
                Console.WriteLine($"Победил {_firstSquad.Name} отряд !");
            }
            else if (_firstSquad.Units.Count == 0 && _secondSquad.Units.Count == 0)
            {
                Console.WriteLine("Поздравляю отряды двух стран убили друг друга, никто не победил и все проиграли");
            }
        }

        private void AttackSquad()
        {
            List<CombatUnit> firstSquad = _firstSquad.Units;
            List<CombatUnit> secondSquad = _secondSquad.Units;

            while (firstSquad.Count > 0 && secondSquad.Count > 0)
            {
                BattleSquads(firstSquad, secondSquad);
                BattleSquads(secondSquad, firstSquad);
            }
        }

        private void BattleSquads(List<CombatUnit> _attackSquad, List<CombatUnit> _defendSquad)
        {
            int firstDefender = 0;

            for (int i = 0; i < _attackSquad.Count; i++)
            {
                if (_attackSquad.Count >= _defendSquad.Count)
                {
                    if (i >= _defendSquad.Count)
                    {
                        UnitAttackRange(_attackSquad, _defendSquad, i, firstDefender);
                    }
                    else
                    {
                        UnitAttackRange(_attackSquad, _defendSquad, i, i);
                    }
                }
                else if (_attackSquad.Count < _defendSquad.Count)
                {
                    UnitAttackRange(_attackSquad, _defendSquad, i, i);
                }
            }
        }

        private void UnitAttackRange(List<CombatUnit> _attackSquad, List<CombatUnit> _defendSquad, int idUnit, int temporary)
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
        public List<CombatUnit> Units = new List<CombatUnit>();
        private bool _isCorrectCreate;
        private ConsoleKey _key;
        private List<CombatUnit> _listCombatUnits = new List<CombatUnit>();

        public Squad()
        {
            _isCorrectCreate = false;
            _key = ConsoleKey.F;
            _listCombatUnits.Add(new ArmoredVehicle(Wars.NameUnits.БТР, 160, 35, 120, 5, true, 2, 0));
            _listCombatUnits.Add(new Infantry(Wars.NameUnits.Пехотинец, 50, 30, 50, 5, true, 2, 0));
            _listCombatUnits.Add(new Sapper(Wars.NameUnits.Сапер, 60, 20, 40, 5, true, 1, 0));
            _listCombatUnits.Add(new Sniper(Wars.NameUnits.Снайпер, 40, 10, 10, 5, true, 4, 0));
            _listCombatUnits.Add(new Tank(Wars.NameUnits.Танк, 200, 45, 200, 5, true, 3, 0));
            _listCombatUnits.Add(new Artelerist(Wars.NameUnits.Артилерист, 100, 20, 10, 5, true, 5, 0));
        }

        public string Name { get; private set; }

        public void ShowUnits(List<CombatUnit> _unitsSquad)
        {
            for (int i = 0; i < _unitsSquad.Count; i++)
            {
                Console.WriteLine($"Номер юнита - {i + 1},Имя юнита - {_unitsSquad[i].Name}, здоровье юнита - {_unitsSquad[i].Health}, броня юнита - {_unitsSquad[i].Armor}, наносимый урон - {_unitsSquad[i].Damage}\n,необходимое расстояние для атаки - {_unitsSquad[i].DistanceAttackUnit}");
            }
        }

        public void RecruitmentUnit()
        {
            Console.WriteLine("Перед созданием отряда придумайте ему пафосное название");
            Name = Console.ReadLine();

            while (_isCorrectCreate == false)
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
                    Units.Add(_listCombatUnits[inputID - 1].Clone());
                }

                Console.WriteLine($"Что бы закончить выбор  бойцов нажмите на {_key}, иначе любую кнопку");

                if (Console.ReadKey().Key == _key)
                {
                    _isCorrectCreate = true;
                }

                Console.Clear();
            }

            Console.Clear();
        }
    }

    abstract class CombatUnit
    {
        protected int TimeStandPoint;
        private float DamageResistence;

        public CombatUnit(Enum name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit, int standCoin)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
            Distance = distance;
            IsLive = isLive;
            DistanceAttackUnit = distanceAttackUnit;
            TimeStandPoint = standCoin;
        }

        public Enum Name { get; private set; }
        public bool IsLive { get; private set; }
        public int DistanceAttackUnit { get; private set; }
        public int Distance { get; private set; }
        public float Health { get; private set; }
        public float Armor { get; protected set; }
        public float Damage { get; protected set; }

        public void CauseDamage(CombatUnit unit)
        {
            float finalDamage = 0;
            float finalArmor = Armor * DamageResistence;

            unit.Attack(this);

            if (Armor <= 0)
            {
                finalDamage = unit.Damage;
                Health -= finalDamage;
            }
            else if (unit.Damage >= finalArmor)
            {
                finalDamage = unit.Damage - finalArmor;
                Armor = 0;
                Health -= finalDamage;
            }
            else if (unit.Damage < finalArmor)
            {
                finalDamage = unit.Damage - finalArmor;
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

        public abstract CombatUnit Clone();

        protected virtual void Attack(CombatUnit unit) { }

        private void CheckDeath()
        {
            if (Health <= 0)
            {
                IsLive = false;
            }
        }
    }

    class Sniper : CombatUnit
    {
        private int _debaffArmor = 10;
        private int _timeStandEnemyUnit = 1;

        public Sniper(Enum name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit, int timeStandPoint) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit, timeStandPoint) { }

        public override CombatUnit Clone()
        {
            return new Sniper(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit, TimeStandPoint);
        }

        protected override void Attack(CombatUnit unit)
        {
            switch (unit.Name)
            {
                case NameUnits.Снайпер:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackRangeUnit(unit);
                    break;
                case NameUnits.Танк:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                case NameUnits.Пехотинец:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                default:
                    MoveUnit();
                    break;
            }
        }

        private void SpellAttackMeleUnit(CombatUnit unit)
        {
            unit.UpTimeStandPoint(_timeStandEnemyUnit);
        }

        private void SpellAttackRangeUnit(CombatUnit unit)
        {
            unit.DebafResistence(_debaffArmor);
        }
    }

    class Tank : CombatUnit
    {
        private float _bonusDamage = 20;
        private float _baseDamage;
        private int _timeUseSkill = 2;
        private int _temporaryCoinUseSkill = 0;

        public Tank(Enum name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit, int timeStandPoint) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit, timeStandPoint)
        {
            _baseDamage = Damage;
        }

        public override CombatUnit Clone()
        {
            return new Tank(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit, TimeStandPoint);
        }

        protected override void Attack(CombatUnit unit)
        {
            switch (unit.Name)
            {
                case NameUnits.Сапер:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit();
                    break;
                case NameUnits.Артилерист:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackRangeUnit();
                    break;
                case NameUnits.БТР:
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

    class Artelerist : CombatUnit
    {
        private int _chanceHit = 0;
        private int _missShoot = 0;
        private int _hitShootEnemy = 2;
        private Random random = new Random();

        public Artelerist(Enum name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit, int timeStandPoint) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit, timeStandPoint) { }

        public override CombatUnit Clone()
        {
            return new Artelerist(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit, TimeStandPoint);
        }

        protected override void Attack(CombatUnit unit)
        {
            switch (unit.Name)
            {
                case NameUnits.Сапер:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                case NameUnits.Пехотинец:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                case NameUnits.Танк:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                default:
                    MoveUnit();
                    break;
            }
        }

        private void SpellAttackMeleUnit(CombatUnit unit)
        {
            _chanceHit = random.Next(_missShoot, _hitShootEnemy);

            if (_chanceHit == _hitShootEnemy)
            {
                unit.KillUnit();
            }
        }
    }

    class Infantry : CombatUnit
    {
        private int _upDistanceAttack = 3;
        private int _coinUpDistanceAttack = 0;
        private int _timeStandPoint = 2;

        public Infantry(Enum name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit, int timeStandPoint) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit, timeStandPoint) { }

        public override CombatUnit Clone()
        {
            return new Infantry(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit, TimeStandPoint);
        }

        protected override void Attack(CombatUnit unit)
        {
            switch (unit.Name)
            {
                case NameUnits.Сапер:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                case NameUnits.Артилерист:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackRangeUnit(unit);
                    break;
                case NameUnits.БТР:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                default:
                    MoveUnit();
                    break;
            }
        }

        private void SpellAttackMeleUnit(CombatUnit unit)
        {
            if (_coinUpDistanceAttack == _upDistanceAttack)
            {
                Console.WriteLine("За родину,Бойцы воодушивились и на целились на врага дистанция для атаки увеличилась");
                UpAttackDistance(unit.Distance);
                _coinUpDistanceAttack = 0;
            }

            _coinUpDistanceAttack++;
        }

        private void SpellAttackRangeUnit(CombatUnit unit)
        {
            Console.WriteLine("Кидает гранату оглушает противника");
            unit.UpTimeStandPoint(_timeStandPoint);
        }
    }

    class ArmoredVehicle : CombatUnit
    {
        private int _amountOfCoinUseSkill = 3;
        private float _buffArmor = 10;
        private int _minimumCoinUseSkill = 0;

        public ArmoredVehicle(Enum name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit, int timeStandPoint) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit, timeStandPoint) { }

        public override CombatUnit Clone()
        {
            return new ArmoredVehicle(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit, TimeStandPoint);
        }

        protected override void Attack(CombatUnit unit)
        {
            switch (unit.Name)
            {
                case NameUnits.Сапер:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit();
                    break;
                case NameUnits.Пехотинец:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit();
                    break;
                case NameUnits.Танк:
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
            if (_amountOfCoinUseSkill > _minimumCoinUseSkill)
            {
                Console.WriteLine("Скорость наше все пока мы едем броня увеличена");
                MoveUnit();
                Armor += _buffArmor;
            }
        }
    }

    class Sapper : CombatUnit
    {
        private float _bonusDamageAttack = 20;
        private float _debafResistence = 10;

        public Sapper(Enum name, float health, float damage, float armor, int distance, bool isLive, int distanceAttackUnit, int timeStandPoint) : base(name, health, damage, armor, distance, isLive, distanceAttackUnit, timeStandPoint) { }

        public override CombatUnit Clone()
        {
            return new Sapper(Name, Health, Damage, Armor, Distance, IsLive, DistanceAttackUnit, TimeStandPoint);
        }

        protected override void Attack(CombatUnit unit)
        {
            switch (unit.Name)
            {
                case NameUnits.БТР:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                case NameUnits.Пехотинец:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                case NameUnits.Танк:
                    if (SuccessfukApplicationSkillAttack())
                        SpellAttackMeleUnit(unit);
                    break;
                default:
                    MoveUnit();
                    break;
            }
        }

        private void SpellAttackMeleUnit(CombatUnit unit)
        {
            if (Distance == unit.DistanceAttackUnit)
            {
                Console.WriteLine("Враг на дистанции атаки, достаем гранатомёт и атакуем.Сопротивление врага уменьшилась, а наносимый урон увеличился");
                unit.DebafResistence(_debafResistence);
                UpDamageAttack(_bonusDamageAttack);
            }
        }
    }
}