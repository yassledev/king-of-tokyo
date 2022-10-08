using KingOfTokyo.Models.Dices;
using KingOfTokyo.Models.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTokyo.Models.Cards
{
    public class CardPower : Card
    {

        public CardPower(int _id, string _name, string _description, int _price) : base(_id, _name, _description, _price){}

        public override void activateEffect(Monster m, List<Monster> monsters)
        {

            //todo

            switch (Id)
            {
                case 19:
                    break;
                case 20:
                    break;
                case 21:
                    m.VictoryPoint++;
                    break;
                case 22:
                    break;
                case 23:
                    foreach (Card c in m.Cards)
                        c.Price--;
                    break;
                case 24:
                    break;
                case 25:
                    break;
                case 26:
                    if (m.Location == Location.NamekWorld || m.Location == Location.EarthWorld)
                        m.VictoryPoint++;
                    break;
                case 27:
                    if (m.Health == 0)
                    {
                        m.Cards.Clear();
                        m.VictoryPoint = 0;
                        m.Location = Location.Outside;
                        m.Health = 10;
                    }
                    break;
                case 28:
                    m.VictoryPoint += 2;
                    break;
                case 29:
                    m.AttackPoint++;
                    break;
                case 30:
                    break;
                case 31:
                    break;
                case 32:
                    m.Energy++;
                    break;
                case 33:
                    m.Energy -= 2;
                    m.Health++;
                    break;
                case 34:
                    m.AttackPoint += 2;
                    break;
                case 35:
                    m.Energy--;
                    break;
                case 36:
                    m.Energy -= 2;
                    break;
                case 37:
                    foreach (Card c in m.Cards)
                    {
                        m.Energy += c.Price;
                        m.Cards.Remove(c);
                    }
                    break;
                case 38:
                    break;
                case 39:
                    foreach (Monster monster in monsters)
                    {
                        if (monster.Health == 0)
                            monster.VictoryPoint += 3;
                    }
                    break;
                case 40:
                    m.Health++;
                    break;
                case 41:

                    break;
                case 42:
                    break;
                case 43:
                    m.Energy++;
                    break;
                case 44:
                    break;
                case 45:
                    m.VictoryPoint++;
                    break;
                case 46:
                    m.Health += 2;
                    break;
                case 47:
                    m.Health += m.Energy % 6;
                    break;
                case 48:
                    m.VictoryPoint++;
                    break;
                case 49:
                    if (m.Energy == 0)
                        m.Energy++;
                    break;
                case 50:
                    if (m.Location == Location.NamekWorld || m.Location == Location.EarthWorld)
                        m.AttackPoint++;
                    break;
                case 51:
                    break;
                case 52:
                    m.AttackPoint += 9;
                    break;
                case 53:
                    break;
                case 54:
                    m.AttackPoint++;
                    break;
                case 55:
                    break;
                case 56:
                    if (DiceRoller.Attack >= 1)
                    {
                        foreach (Monster monster in monsters)
                            monster.Health--;
                    }
                    break;
                case 57:
                    break;
                case 58:
                    break;
                case 59:
                    break;
                case 60:
                    break;
                case 61:
                    break;
                case 62:
                    m.VictoryPoint += 2;
                    break;
                case 63:
                    foreach (Monster monster in monsters)
                        if (m.Health >= monster.Health)
                            break;
                    m.Health++;
                    break;
                case 64:
                    break;
                default:
                    break;
            }
        }

    }
}
