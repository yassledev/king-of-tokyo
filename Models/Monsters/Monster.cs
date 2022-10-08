using KingOfTokyo.Models.Dices;
using System;
using System.Collections.Generic;
using System.Text;
using KingOfTokyo.Models.Cards;
using KingOfTokyo.Models.Game;

namespace KingOfTokyo.Models.Monsters
{
    public class Monster : Player
    {
        private int health = 10;
        public int Health
        {
            get { return health; }
            set { health = value > 10 ? 10 : value; }
        }
        public int VictoryPoint { get; set; } = 0;
        public int Energy { get; set; } = 0;
        public int AttackPoint { get; set; } = 0;
        public bool IsAlive => Health > 0;
        public Location Location { get; set; } = Location.Outside;
        public List<Card> Cards = new List<Card>();
        public Monster(int id, string pseudo) : base(id, pseudo){}

        public void BuyCard(Card card)
        {
            if (Energy < card.Price) return;

            Energy -= card.Price;

            if (card is CardAction) {
                if (card.Id == 9 && this.Location != Location.NamekWorld)
                {
                    Board.NamekWorld.Location = Location.Outside;
                    Board.NamekWorld = this;
                    this.Location = Location.NamekWorld;

                }
                card.activateEffect(this, Jeu.Monsters);
                return;
            }

            Cards.Add(card);
        }
        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
        }

        public void SellCard(Monster monster, Card card)
        {
            RemoveCard(card);
            Energy += card.Price;
            monster.BuyCard(card);
        }
    }
}
