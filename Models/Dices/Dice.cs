using System;
using System.Collections.Generic;
using System.Text;

namespace KingOfTokyo.Models.Dices
{
    public enum Color { Green, Black }
    public enum Symbol { One, Two, Three, Energy, Attack, Health }
    [Serializable]
    public class Dice
    {
        private readonly Random Random;
        public Symbol Symbol;
        public Color Color;
        // Player want save dice 
        public bool Save { get; set; }

        public Dice(Color color = Color.Black)
        {
            Color = color;
            Save = false;
            Random = new Random(Guid.NewGuid().GetHashCode());
        }

        public void Roll()
        {
            if (!Save)
            {
                Symbol = (Symbol)Random.Next(0, 6); // 6 Faces of dice
            }
        }
    }
}
