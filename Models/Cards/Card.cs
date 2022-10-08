using System;
using System.Collections.Generic;
using System.Text;
using KingOfTokyo.Models.Monsters;

namespace KingOfTokyo
{
    public abstract class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        private static Random rng;
        static Card()
        {
            rng = new Random();
        }

        public Card(int _id, string _name, string _description, int _price)
        {
            Id = _id;
            Name = _name;
            Description = _description;
            Price = _price;
            rng = new Random();
        }


        public static List<Card> GetShuffleCards<Card>(List<Card> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

        public abstract void activateEffect(Monster m, List<Monster> monsters);

    }
}
