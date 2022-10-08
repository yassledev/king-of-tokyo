using KingOfTokyo.Models.Monsters;
using System;
using System.Collections.Generic;
using System.Text;

namespace KingOfTokyo.Models.Game
{
    public static class Board { 
    
        public static Monster NamekWorld { get; set; }
        public static Monster EarthWorld { get; set; }

        public static bool NamekWorldOccupied => NamekWorld != null;
        public static bool EarthWorldOccupied => EarthWorld != null;
        
        public static bool WorldOccupied(Monster monster) {
            return monster == NamekWorld || monster == EarthWorld;
        }
    }
}
