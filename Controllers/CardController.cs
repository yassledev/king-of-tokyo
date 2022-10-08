using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingOfTokyo.Models.Game;

namespace KingOfTokyo.Controllers
{
    public static class CardController
    {
        public static Card TopOfDeck()
        {
            return Jeu.Deck.Count != 0 ? Jeu.Deck.Peek() : null;
        }

        public static Card CardForSaleOne()
        {
            return Jeu.CardsForSale.Count > 0 ? Jeu.CardsForSale[0] : null;
        }

        public static Card CardForSaleTwo()
        {
            return Jeu.CardsForSale.Count > 1 ? Jeu.CardsForSale[1] : null;
        }

        public static Card CardForSaleThree()
        {
            return Jeu.CardsForSale.Count > 2 ? Jeu.CardsForSale[2] : null;
        }

        public static void BuyCardOne()
        {
            if (CardForSaleOne() != null) Jeu.BuyCard(0);
        }

        public static void BuyCardTwo()
        {
            if (CardForSaleTwo() != null) Jeu.BuyCard(1);
        }

        public static void BuyCardThree()
        {
            if (CardForSaleThree() != null) Jeu.BuyCard(2);
        }

    }
}
