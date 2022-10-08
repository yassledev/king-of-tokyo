using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace KingOfTokyo.Models.Dices
{
    public static class DiceRoller
    {
        public static int[] Faces = new int[6];

        public static List<Dice> Dices = new List<Dice>();
        public static int Attack => Faces[(int)Symbol.Attack];
        public static int Energy => Faces[(int)Symbol.Energy];
        public static int Heal => Faces[(int)Symbol.Health];
        public static int VictoryPoints => GetVictoryPoint();

        public static void Setup()
        {
            Dices.Clear();
            for(int i = 0; i < 6; i++)
            {
                Dices.Add(new Dice());
            }
        }

        public static void Roll()
        {
            Array.Clear(Faces, 0, Faces.Length);
            foreach(Dice d in Dices)
            {
                d.Roll();
                Faces[(int)d.Symbol]++;
            }
        }

        public static void ResolveDice()
        {
            Array.Clear(Faces, 0, Faces.Length);
            foreach (Dice d in Dices)
                Faces[(int)d.Symbol]++;
        }

        // 3 Faces identiques = 1 pts + nombre de symbole en plus
        public static int GetVictoryPoint()
        {
            int[] symbol = { (int)Symbol.One, (int)Symbol.Two, (int)Symbol.Three };
            int somme = 0;
            foreach (int s in symbol)
                if (Faces[s] >= 3)
                    somme += (s + 1) + (Faces[s] - 3);
            return somme;
        }
    }
}
