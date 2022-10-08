using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KingOfTokyo.Models.Monsters
{
    [Serializable]
    public class Player
    {
        public int Id { get; set; } = -1;
        public string Pseudo { get; set; }

        public Player(int id, string pseudo)
        {
            Id = id;
            Pseudo = pseudo;
        }

        override
        public string ToString()
        {
            return "Id : " + Id + " | Pseudo: " + Pseudo;
        }
        
    }
}
