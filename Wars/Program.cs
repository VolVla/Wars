using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace Wars
{
    internal class Program
    {
        static void Main(string[] args)
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
        public void TryCreativeBattle()
        {
            _firstSquad.CreativeSquad();
            _secondSquad.CreativeSquad();
        }

        public void Battle()
        {
         //   while (_fightFighters[_firstFighter].Health > 0 && _fightFighters[_secondFighter].Health > 0)
         //   {
          //      _fightFighters[_firstFighter].CauseDamage(_fightFighters[_secondFighter]);
          //      _fightFighters[_secondFighter].CauseDamage(_fightFighters[_firstFighter]);
          //  }
        }
        public void AnnounceWinner()
        {
           // if (_fightFighters[_firstFighter].Health <= 0)
           // {
       //         Console.WriteLine($"Победил {_fightFighters[_secondFighter].Name} !");
         //   }
        //        else if (_fightFighters[_secondFighter].Health <= 0)
          //  {
         //       Console.WriteLine($"Победил {_fightFighters[_firstFighter].Name} !");
       //     }
//else if (_fightFighters[_firstFighter].Health <= 0 && _fightFighters[_secondFighter].Health <= 0)
         //   {
       //         Console.WriteLine("Поздравляю бойцы убили друг друга, никто не победил и все проиграли");
       //     }
        }

    }

    class Squad
    {
       List<CombatUnit> squad = new List<CombatUnit>();
       public void CreativeSquad()
       {

       }
    }

    abstract class CombatUnit
    {
       private List <CombatUnit> _squad = new List<CombatUnit>();
        public CombatUnit(string name, float health, float damage, float armor)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
        }

        public string Name { get; protected set; }
        public float Health { get; protected set; }
        public float Armor { get; protected set; }
        public float Damage { get; protected set; }
    
    }

    abstract class MeleвDamageUnit : CombatUnit
    {
        List<MeleвDamageUnit> _meleвDamageUnits = new List<MeleвDamageUnit>();
        public MeleвDamageUnit(string name, float health, float damage, float armor): base(name, health, damage, armor) 
        {
            _meleвDamageUnits.Add(new ArmoredVehicle("", 100 , 100, 20));
            _meleвDamageUnits.Add(new infantry("", 100, 100, 20));
        }

    }

    abstract class RangeDamageUnit : CombatUnit
    {
        List<RangeDamageUnit> _rangeDamageUnits = new List<RangeDamageUnit>();
        public RangeDamageUnit(string name, float health, float damage, float armor) : base(name, health, damage, armor)
        {
            _rangeDamageUnits.Add(new Sniper("", 100, 100, 20));
            _rangeDamageUnits.Add(new Tank("", 100, 100, 20));
            _rangeDamageUnits.Add(new Sniper("", 100, 100, 20));
        }
    }

    class Sniper : RangeDamageUnit
    {
        public Sniper(string name, float health, float damage, float armor) : base(name, health, damage, armor) { }
    }

    class Tank : RangeDamageUnit
    {
        public Tank(string name, float health, float damage, float armor) : base(name, health, damage, armor) { }
    }

    class Artelerist : RangeDamageUnit
    {
        public Artelerist(string name, float health, float damage, float armor) : base(name, health, damage, armor) { }
        
    }
    

    class infantry : MeleвDamageUnit
    {
        public infantry(string name, float health, float damage, float armor) : base(name, health, damage, armor)  { }
    }

    class ArmoredVehicle : MeleвDamageUnit
    {
        public ArmoredVehicle(string name, float health, float damage, float armor) : base(name, health, damage, armor) { }
    }

    class Sapper : MeleвDamageUnit
    {
        public Sapper(string name, float health, float damage, float armor) : base(name, health, damage, armor) { }
    }

}
