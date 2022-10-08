using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingOfTokyo.Models.Cards;
using KingOfTokyo.Models.Monsters;
using KingOfTokyo.Models.Game;

namespace KingOfTokyo.Models.Cards
{
    public class CardAction : Card
    {
        public int Energy { get; set; }
        public int EnnemyEnergy { get; set; }
        public int Health { get; set; }
        public int EnnemyHealth { get; set; }
        public int VictoryPoint { get; set; }
        public int EnnemyVictoryPoint { get; set; }
        public bool ControlNamek { get; set; }

        public CardAction(int _id, string _name, string _description, int _price, int _energy, int _ennemyEnergy, int _health, int _ennemyHealth, int _victoryPoint, int _ennemyVictoryPoint, bool  _controlNamek) : base(_id, _name, _description, _price)
        {
            Energy = _energy;
            EnnemyEnergy = _ennemyEnergy;
            Health = _health;
            EnnemyHealth = _ennemyHealth;
            VictoryPoint = _victoryPoint;
            EnnemyVictoryPoint = _ennemyVictoryPoint;
            ControlNamek = _controlNamek;
        }

       public override void activateEffect(Monster m, List<Monster> monsters)
        {
            
            m.Energy += Energy;
            m.Health += Health;
            m.VictoryPoint += VictoryPoint;

            foreach (Monster monster in monsters)
            {
                if (monster != m)
                {
                    monster.Health += EnnemyHealth;
                    monster.Energy += EnnemyEnergy;
                    monster.VictoryPoint += EnnemyVictoryPoint;
                }
            }


            if (ControlNamek)
            {
                Board.NamekWorld.Location = Location.Outside;
                m.Location = Location.NamekWorld;
                Board.NamekWorld = m;

            }
        }




    }
}
