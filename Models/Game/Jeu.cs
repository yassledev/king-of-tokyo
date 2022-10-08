using KingOfTokyo.Models.Dices;
using KingOfTokyo.Models.Monsters;
using System.Collections.Generic;
using System.Linq;
using KingOfTokyo.Models.Cards;
using System;
using System.Windows;

namespace KingOfTokyo.Models.Game
{
    public static class Jeu
    {
        public static List<Player> Players { get; set; } = new List<Player>();
        public static List<Monster> Monsters { get; set; } = new List<Monster>();
        public static List<Monster> Attacked { get; set; } = new List<Monster>();

        public static Monster CurrentMonster { get; set; }
        public static Player Host { get; set; }
        public static Monster Winner { get; set; }
        public static int MaxRoll { get; set; } = 3;
        public static int IndexCurrent { get; set; } = -1; // -1 to start at 0
        public static State State {get; set;}

        public static List<Card> Cards { get; set; } = new List<Card>();
        public static Stack<Card> Deck;
        public static List<Card> CardsForSale = new List<Card>();

        public static void addCards()
        {
            Cards.Add(new CardAction(1, "Lance-flammes", "Tous les autres personnages perdent 2 coeurs", 3, 0, 0, 0, -2, 0, 0, false));
            Cards.Add(new CardAction(2, "Soin", "Vous gagnez 2 coeurs", 3, 0, 0, 0, -2, 0, 0, false));
            Cards.Add(new CardAction(3, "La petite boutique du coin", "Vous gagnez 1 point de victoire", 3, 0, 0, 2, 0, 0, 0, false));
            Cards.Add(new CardAction(4, "Garde national", "Vous gagnez 2 points de victoire et vous perdez 2 coeurs", 3, 0, 0, -2, 0, 2, 0, false));
            Cards.Add(new CardAction(5, "Tank", "Vous gagnez 4 points de victoire et vous perdez 3 coeurs", 4, 0, 0, -3, 0, 4, 0, false));
            Cards.Add(new CardAction(6, "Bombardement aérien", "Tous les personnages perdent 3 coeurs (vous y compris)", 3, 0, 0, -3, -3, 0, 0, false));
            Cards.Add(new CardAction(7, "Tramway", "Vous gagnez 2 points de victoire", 4, 0, 0, 0, 0, 2, 0, false));
            Cards.Add(new CardAction(8, "Gratte-ciel", "Vous gagnez 4 points de victoire", 4, 0, 0, 0, 0, 4, 0, false));
            Cards.Add(new CardAction(9, "La mort vient du ciel", "Vous gagnez 2 points de victoire et vous prenez le contrôle de Namek si vous n'y êtes pas déjà", 6, 0, 0, 0, 0, 2, 0, true));
            Cards.Add(new CardAction(10, "Centrale nucléaire", "Vous gagnez 2 points de victoire et tous les autres personnages perdent 3 coeurs", 5, 0, 0, 3, 0, 2, 0, false));
            Cards.Add(new CardAction(11, "Raffinerie de gaz", "Vous gagnez 2 points de victoire et tous les autres personnages perdent 3 coeurs", 6, 0, 0, 0, -3, 2, 0, false));
            Cards.Add(new CardAction(12, "Ordre d'évacuation", "Tous les autres personnages perdent 5 points de victoire", 7, 0, 0, 0, 0, 0, -5, false));
            Cards.Add(new CardAction(13, "Ordre d'évacuation", "Tous les autres personnages perdent 5 points de victoire", 7, 0, 0, 0, 0, 0, -5, false));
            Cards.Add(new CardAction(14, "Recharge", "Vous gagnez 9 points d'énergie", 8, 9, 0, 0, 0, 0, 0, false));
            Cards.Add(new CardAction(15, "Immeuble", "Vous gagnez 3 points de victoire", 5, 0, 0, 0, 0, 3, 0, false));
            Cards.Add(new CardAction(16, "Avions de chasse", "Vous gagnez 5 points de victoire et vous perdez 4 coeurs", 5, 0, 0, -4, 0, 5, 0, false));
            Cards.Add(new CardAction(17, "Tempête magnétique", "Vous gagnez 2 points de victoire et tous les autres personnages perdent 1 point d'énergie pour chaque 2 points d'énergie qu'ils possèdent", 6, 0, 0, 0, 0, 2, 0, false));
            //Cards.Add(new CardAction(18, "Rage", "Jouer un autre tour après celui-là", 7, 0, 0, 0, 0, 0, 0, false));
            Cards.Add(new CardAction(1, "Lance-flammes", "Tous les autres personnages perdent 2 coeurs", 3, 0, 0, 0, -2, 0, 0, false));
            Cards.Add(new CardAction(2, "Soin", "Vous gagnez 2 coeurs", 3, 0, 0, 0, -2, 0, 0, false));
            Cards.Add(new CardAction(3, "La petite boutique du coin", "Vous gagnez 1 point de victoire", 3, 0, 0, 2, 0, 0, 0, false));
            Cards.Add(new CardAction(4, "Garde national", "Vous gagnez 2 points de victoire et vous perdez 2 coeurs", 3, 0, 0, -2, 0, 2, 0, false));
            Cards.Add(new CardAction(5, "Tank", "Vous gagnez 4 points de victoire et vous perdez 3 coeurs", 4, 0, 0, -3, 0, 4, 0, false));
            Cards.Add(new CardAction(6, "Bombardement aérien", "Tous les personnages perdent 3 coeurs (vous y compris)", 3, 0, 0, -3, -3, 0, 0, false));
            Cards.Add(new CardAction(7, "Tramway", "Vous gagnez 2 points de victoire", 4, 0, 0, 0, 0, 2, 0, false));
            Cards.Add(new CardAction(8, "Gratte-ciel", "Vous gagnez 4 points de victoire", 4, 0, 0, 0, 0, 4, 0, false));
            Cards.Add(new CardAction(9, "La mort vient du ciel", "Vous gagnez 2 points de victoire et vous prenez le contrôle de Namek si vous n'y êtes pas déjà", 6, 0, 0, 0, 0, 2, 0, true));
            Cards.Add(new CardAction(10, "Centrale nucléaire", "Vous gagnez 2 points de victoire et tous les autres personnages perdent 3 coeurs", 5, 0, 0, 3, 0, 2, 0, false));
            Cards.Add(new CardAction(11, "Raffinerie de gaz", "Vous gagnez 2 points de victoire et tous les autres personnages perdent 3 coeurs", 6, 0, 0, 0, -3, 2, 0, false));
            Cards.Add(new CardAction(12, "Ordre d'évacuation", "Tous les autres personnages perdent 5 points de victoire", 7, 0, 0, 0, 0, 0, -5, false));
            Cards.Add(new CardAction(13, "Ordre d'évacuation", "Tous les autres personnages perdent 5 points de victoire", 7, 0, 0, 0, 0, 0, -5, false));
            Cards.Add(new CardAction(14, "Recharge", "Vous gagnez 9 points d'énergie", 8, 9, 0, 0, 0, 0, 0, false));
            Cards.Add(new CardAction(15, "Immeuble", "Vous gagnez 3 points de victoire", 5, 0, 0, 0, 0, 3, 0, false));
            Cards.Add(new CardAction(16, "Avions de chasse", "Vous gagnez 5 points de victoire et vous perdez 4 coeurs", 5, 0, 0, -4, 0, 5, 0, false));
            Cards.Add(new CardAction(17, "Tempête magnétique", "Vous gagnez 2 points de victoire et tous les autres personnages perdent 1 point d'énergie pour chaque 2 points d'énergie qu'ils possèdent", 6, 0, 0, 0, 0, 2, 0, false));
            //Cards.Add(new CardAction(18, "Rage", "Jouer un autre tour après celui-là", 7, 0, 0, 0, 0, 0, 0, false));
            Cards.Add(new CardAction(1, "Lance-flammes", "Tous les autres personnages perdent 2 coeurs", 3, 0, 0, 0, -2, 0, 0, false));
            Cards.Add(new CardAction(2, "Soin", "Vous gagnez 2 coeurs", 3, 0, 0, 0, -2, 0, 0, false));
            Cards.Add(new CardAction(3, "La petite boutique du coin", "Vous gagnez 1 point de victoire", 3, 0, 0, 2, 0, 0, 0, false));
            Cards.Add(new CardAction(4, "Garde national", "Vous gagnez 2 points de victoire et vous perdez 2 coeurs", 3, 0, 0, -2, 0, 2, 0, false));
            Cards.Add(new CardAction(5, "Tank", "Vous gagnez 4 points de victoire et vous perdez 3 coeurs", 4, 0, 0, -3, 0, 4, 0, false));
            Cards.Add(new CardAction(6, "Bombardement aérien", "Tous les personnages perdent 3 coeurs (vous y compris)", 3, 0, 0, -3, -3, 0, 0, false));
            Cards.Add(new CardAction(7, "Tramway", "Vous gagnez 2 points de victoire", 4, 0, 0, 0, 0, 2, 0, false));
            Cards.Add(new CardAction(8, "Gratte-ciel", "Vous gagnez 4 points de victoire", 4, 0, 0, 0, 0, 4, 0, false));
            Cards.Add(new CardAction(9, "La mort vient du ciel", "Vous gagnez 2 points de victoire et vous prenez le contrôle de Namek si vous n'y êtes pas déjà", 6, 0, 0, 0, 0, 2, 0, true));
            Cards.Add(new CardAction(10, "Centrale nucléaire", "Vous gagnez 2 points de victoire et tous les autres personnages perdent 3 coeurs", 5, 0, 0, 3, 0, 2, 0, false));
            Cards.Add(new CardAction(11, "Raffinerie de gaz", "Vous gagnez 2 points de victoire et tous les autres personnages perdent 3 coeurs", 6, 0, 0, 0, -3, 2, 0, false));
            Cards.Add(new CardAction(12, "Ordre d'évacuation", "Tous les autres personnages perdent 5 points de victoire", 7, 0, 0, 0, 0, 0, -5, false));
            Cards.Add(new CardAction(13, "Ordre d'évacuation", "Tous les autres personnages perdent 5 points de victoire", 7, 0, 0, 0, 0, 0, -5, false));
            Cards.Add(new CardAction(14, "Recharge", "Vous gagnez 9 points d'énergie", 8, 9, 0, 0, 0, 0, 0, false));
            Cards.Add(new CardAction(15, "Immeuble", "Vous gagnez 3 points de victoire", 5, 0, 0, 0, 0, 3, 0, false));
            Cards.Add(new CardAction(16, "Avions de chasse", "Vous gagnez 5 points de victoire et vous perdez 4 coeurs", 5, 0, 0, -4, 0, 5, 0, false));
            Cards.Add(new CardAction(17, "Tempête magnétique", "Vous gagnez 2 points de victoire et tous les autres personnages perdent 1 point d'énergie pour chaque 2 points d'énergie qu'ils possèdent", 6, 0, 0, 0, 0, 2, 0, false));
            //Cards.Add(new CardAction(18, "Rage", "Jouer un autre tour après celui-là", 7, 0, 0, 0, 0, 0, 0, false));

            //todo gestion des cartes power...
            //Cards.Add(new CardPower(19, "Ninja Urbain", "Vous pouvez toujours relancer les \"3\" que vous obtenez", 4));
            //Cards.Add(new CardPower(20, "Sonde psychique", "Vous pouvez relancer le dé de votre choix après le dernier lancer de chaque adversaire. Si vous obtenez un coeur, défaussez cette carte", 3));
            //Cards.Add(new CardPower(21, "Médiatique", "Gagnez 1 point de victoire chaque fois que vous achetez une carte énergie", 3));
            //Cards.Add(new CardPower(22, "Elastique", "Avant de résoudre vos dés, vous pouvez dépenser 2 points d'énergie pour changer la face d'un de vos dés", 3));
            //Cards.Add(new CardPower(23, "Origine extra-terrestre", "Le prix des cartes énergie que vous achetez est diminué d'1 point d'énergie", 3));
            //Cards.Add(new CardPower(24, "Souffle nova", "Vos baffes blessent tous les autres personnages", 7));
            //Cards.Add(new CardPower(25, "Expérience scientifique", "Pendant votre phase d'achat vous pouvez regarder secrètement la première carte énergie de la pioche et l'acheter ou la reposer sur la pioche", 2));
            //Cards.Add(new CardPower(26, "Urbavore", "Gagnez 1 point de victoire supplémentaire lorsque vous commencez votre tour dans Namek. Si vous êtes dans Namek et que vous obtenez au moins une baffe, ajoutez une baffe à votre résultat", 4));
            //Cards.Add(new CardPower(27, "Il restait un oeuf!", "Si vous atteignez 0 coeurs, défaussez toutes vos cartes, perdez tous vos points de victoire et quittez Namek. Gagnez 10 coeurs et continuez à jouer", 7));
            //Cards.Add(new CardPower(28, "Gourmet", "Gagnez 2 points de victoire supplémentaires lorque vous obtenez au moins 3 dés \"1\"", 4));
            //Cards.Add(new CardPower(29, "Attaque acide", "Ajoutez une baffe à votre résultat", 6));
            //Cards.Add(new CardPower(30, "Rayon réducteur", "Donnez 1 jeton \"Toupetit\" à chaque personnage que vous blessez. Chaque jeton \"Toupetit\" retire un dé au lancer du personnage qui le possède. Un jeton \"Toupetit\" peut être défaussé en utilisant un dé coeur au lieu de gagner un coeur avec", 6));
            //Cards.Add(new CardPower(31, "Armure de plaques", "Ne perdez pas de coeurs lorsque vous devriez perdre exactement 1 coeur", 4));
            //Cards.Add(new CardPower(32, "L'ami des enfants", "Gagnez 1 point d'énergie supplémentaire chaque fois que vous gagnez au moins 1 point d'énergie", 3));
            //Cards.Add(new CardPower(33, "Guérison rapide", "Vous pouvez dépenser 2 points d'énergie à tout moment pour gagner 1 coeur", 3));
            //Cards.Add(new CardPower(34, "Dards empoisonnés", "Lorsque vous obtenez au moins 3 dés \"2\", ajoutez 2 baffes à votre résultat", 3));
            //Cards.Add(new CardPower(35, "Boisson énergisante", "Dépensez 1 point d'énergie pour avoir un lancer supplémentaire", 4));
            //Cards.Add(new CardPower(36, "Ailes", "Dépensez 2 points d'énergie pour ne pas perdre de coeurs ce tour-ci", 6));
            //Cards.Add(new CardPower(37, "Métamorphose", "A la fin de votre tour, vous pouvez défausser les cartes pouvoirs de votre choix pour récupérer leurs prix en énergie", 3));
            //Cards.Add(new CardPower(38, "Camouflage", "Si vous devez perdre des coeurs, lancez un dé par coeur que vous devriez perdre. Chaque dé \"coeur\" obtenu annule la perte d'1 coeur", 3));
            //Cards.Add(new CardPower(39, "Nécrophage", "Gagnez 3 points de victoire chaque fois qu'un personnage descend à 0 coeur", 4));
            //Cards.Add(new CardPower(40, "Régénération", "Lorsque vous gagnez des coeurs, gagnez un coeur supplémentaire", 4));
            //Cards.Add(new CardPower(41, "Monstre batterie", "Lorsque vous achetez monstre batterie, placez 6 points d'énergie de la banque dessus. Au début de chacun de vos tours, transférez 2 points d'énergie de cette carte vers votre réserve. Défaussez cette carte lorsque vous transférez les 2 derniers points d'énergie", 3));
            //Cards.Add(new CardPower(42, "Contrôle du temps", "Si vous obtenez au moins 3 dés \"1\", vous pouvez jouer un nouveau tour en lançant un dé de moins qu'à ce tour", 5));
            //Cards.Add(new CardPower(43, "Nous ne faisons que le rendre plus fort", "Gagnez un point d'énergie chaque fois que vous perdez au moins 2 coeurs", 3));
            //Cards.Add(new CardPower(44, "Ecran de fumée", "Placez 3 jetons fumée sur cette carte. Défaussez un jeton fumée pour avoir un lancer supplémentaire. Défaussez cette carte lorsqu'il n'y a plus de jeton fumée dessus", 4));
            //Cards.Add(new CardPower(45, "Herbivore", "Gagnez 1 point de victoire à la fin de votre tour si vous n'avez fait perdre aucun coeur", 5));
            //Cards.Add(new CardPower(46, "Encore plus grand", "Gagnez 2 coeurs quand vous achetez cette carte. Vous pouvez avoir jusqu'à 12 coeurs tant que vous possédez cette carte", 4));
            //Cards.Add(new CardPower(47, "Réserve d'énergie", "Gagnez 1 point de victoire pour chaque 6 points d'énergie que vous possédez à la fin de votre tour", 3));
            //Cards.Add(new CardPower(48, "Monstre Alpha", "Gagnez 1 point de victoire si vous obtenez au moins un dé \"baffe\"", 5));
            //Cards.Add(new CardPower(49, "Energie solaire", "A la fin de votre tour, gagnez 1 point d'énergie si vous n'en aviez pas", 2));
            //Cards.Add(new CardPower(50, "Fouisseur", "Ajoutez une baffe à votre résultat si vous êtes dans Namek. Quand un personnage vous fait fuir Namek il perd 1 coeur", 5));
            //Cards.Add(new CardPower(51, "Rayon revigorant", "Vous pouvez utiliser vos dés \"coeur\" pour faire gagner des coeurs aux personnages. Chaque personnage doit vous payer 2 points d'énergie (ou 1 point d'énergie si c'est son dernier) pour chaque coeur qu'il gagne ainsi", 4));
            //Cards.Add(new CardPower(52, "Destruction totale", "Si tous vos dés ont une face différente, gagnez 9 points de victoire en plus des effets des dés", 3));
            //Cards.Add(new CardPower(53, "Plan B", "Avant de résoudre vos dés, vous pouvez defausser cette carte pour changer la face d'un de vos dés", 3));
            //Cards.Add(new CardPower(54, "Queue à piquants", "Si vous obtenez au moins un dé \"baffe\", ajoutez une baffe à votre résultat", 5));
            //Cards.Add(new CardPower(55, "Crachat venimeux", "Donnez un jeton poison à chaque personnage que vous blessez. Chaque jeton poison fait perdre 1 coeur au personnage qui le possède à la fin de son tour. Un jeton poison peut être défaussé en utilisant un dé \"coeur\" au lieu de gagner 1 coeur avec", 4));
            //Cards.Add(new CardPower(56, "Souffle de feu", "Vos voisins de table perdent 1 coeur lorque vous obtenez au moins un dé \"baffe\"", 4));
            //Cards.Add(new CardPower(57, "Tentacules parasites", "Vous pouvez acheter les cartes pouvoirs des autres monstres. Vous devez leur payer leur prix en points d'énergie", 4));
            //Cards.Add(new CardPower(58, "Réacteur", "Vous ne perdez pas de coeur quand vous fuyez Namek", 5));
            //Cards.Add(new CardPower(59, "Opportuniste", "Chaque fois qu'une carte énergie est révélée vous pouvez l'acheter immédiatement", 3));
            //Cards.Add(new CardPower(60, "Prédateur sélectif", "Vous pouvez changer la face d'un de vos dés en dé \"1\" avant de le résoudre", 3));
            //Cards.Add(new CardPower(61, "Mimétisme", "Choisissez une carte pouvoir devant un personnage et placez le jeton mimétisme dessus. Mimétisme devient une copie de cette carte comme si vous veniez de l'acheter. Dépensez 1 point d'énergie au début de votre tour pour déplacer le jeton mimétisme et copier une autre carte pouvoir", 8));
            //Cards.Add(new CardPower(62, "Détritivore", "Une fois par tour, gagnez 2 points de victoire si vous avez obtenu les dés \"1\", \"2\" et \"3\". Vous pouvez également utiliser ces dés dans d'autres combinaisons", 4));
            //Cards.Add(new CardPower(63, "Trop mignon", "Gagnez 1 point de victoire à la fin de votre tour si vous êtes le personnage avec le moins de point de victoire", 3));
            //Cards.Add(new CardPower(64, "Cerveau démesuré", "Vous avez un lancer supplémentaire", 5));
            //Cards.Add(new CardPower(65, "Une tête de plus", "Jouez avec un dé supplémentaire", 7));
            //Cards.Add(new CardPower(66, "Une tête de plus", "Jouez avec un dé supplémentaire", 7));
        }

        public static void ClearGame()
        {
            Players.Clear();
            Monsters.Clear();
            Attacked.Clear();
            Board.EarthWorld = null;
            Board.NamekWorld = null;
            CurrentMonster = null;
            MaxRoll = 3;
            Winner = null;
            IndexCurrent = -1;
        }

        public static void BuyCard(int index)
        {
            if (index < 0 || index >= CardsForSale.Count) return;
            var card = CardsForSale[index];
            if (CurrentMonster.Energy < card.Price)
            {
                MessageBox.Show("Vous n'avez pas assez de points d'énergie");
                return;
            }
            CardsForSale.RemoveAt(index);
            CurrentMonster.BuyCard(card);
            if (Deck.Count != 0) CardsForSale.Add(Deck.Pop());
        }

        public static void SellCard(Monster monster, Card card)
        {
            State = State.SellingCard;
            CurrentMonster.SellCard(monster, card);
        }

        public static void RemoveCard(Card card)
        {
            State = State.RemovingCard;
            CurrentMonster.RemoveCard(card);
        }

        public static void StartGame()
        {
            addCards();
            Deck = new Stack<Card>(Card.GetShuffleCards(Cards));
            for (int i = 0; i < 3; i++) if (Deck.Count != 0) CardsForSale.Add(Deck.Pop());
            // Create list of monster
            foreach (Player p in Players)
            {
                Monster monster = new Monster(p.Id, p.Pseudo);
                Monsters.Add(monster);
            }
            StartTurn();
        }

        public static void NextMonster()
        {
            do
            {
                IndexCurrent = (IndexCurrent + 1) % Monsters.Count;
                CurrentMonster = Monsters[IndexCurrent];
            } while (!CurrentMonster.IsAlive);
        }

        public static void StartTurn()
        {
            NextMonster();
            DiceRoller.Setup();

            // If in namek or earth during starting turn, get 2 victory pts
            if (Board.WorldOccupied(CurrentMonster))
                CurrentMonster.VictoryPoint += 2;

            // Set next game state
            Jeu.State = State.Roll;

            GetWinner();
        }

        public static void Roll()
        {
            MaxRoll--;
            if(MaxRoll >= 0)
                DiceRoller.Roll();

        }

        public static void RollS()
        {
            if (MaxRoll >= 0)
                DiceRoller.Roll();
        }

        public static void EndRoll()
        {
            CurrentMonster.AttackPoint = DiceRoller.Attack;
            CurrentMonster.Energy += DiceRoller.Energy;
            CurrentMonster.VictoryPoint += DiceRoller.VictoryPoints;

            // If monster is in world, he cant heal
            if(!Board.WorldOccupied(CurrentMonster))
                CurrentMonster.Health += DiceRoller.Heal;

            // CurrentMonster can attack only if he have attack pts
            if (CurrentMonster.AttackPoint > 0)
                Attack();

            // If played in namek is attacked, he can leave city
            if (Attacked.Count > 0)
                State = State.ExitWorld;
            else
                State = State.Card;

            MaxRoll = 3;

            GetWinner();
        }

        public static void ExitNamek(bool exit)
        {
            if (exit)
            {
                if(Board.NamekWorld == Attacked[0])
                {
                    // If current monster outside, he win victory point
                    // If he is in earth, remove location
                    if (CurrentMonster.Location == Location.Outside)
                        CurrentMonster.VictoryPoint += 1;
                    else if (CurrentMonster.Location == Location.EarthWorld)
                        Board.EarthWorld = null;

                    Board.NamekWorld.Location = Location.Outside;
                    Board.NamekWorld = CurrentMonster;
                    CurrentMonster.Location = Location.NamekWorld;
                }
                else
                {
                    // If Current monster located in namek, only remove player from earth
                    if (CurrentMonster.Location != Location.NamekWorld)
                    {
                        Board.EarthWorld.Location = Location.Outside;
                        Board.EarthWorld = CurrentMonster;
                        CurrentMonster.Location = Location.EarthWorld;
                        CurrentMonster.VictoryPoint += 1;
                    }
                    else
                    {
                        Board.EarthWorld.Location = Location.Outside;
                        Board.EarthWorld = null;
                    }

                }
                Attacked.Clear();
            }
            else
            {
                // Remove first monster
                Attacked.RemoveAt(0);
            }

            // Set card state
            if (Attacked.Count == 0)
                State = State.Card;
        }

        public static void EnterCity(Monster m)
        {
            if(Board.NamekWorld == null)
            {
                Board.NamekWorld = m;
                m.Location = Location.NamekWorld;
                m.VictoryPoint += 1;
            }
            else if((Array.FindAll(Monsters.ToArray(), (Monster mm) => mm.IsAlive).Length > 4) && (!Board.EarthWorldOccupied))
            {
                Board.EarthWorld = m;
                m.Location = Location.EarthWorld;
                m.VictoryPoint += 1;
            }
        }

        public static void Attack()
        {
            
            if (Board.WorldOccupied(CurrentMonster))
            {
                // Attack All monster outside
                foreach (Monster m in Monsters)
                    if ((m.Location == Location.Outside) && m.IsAlive)
                        m.Health -= CurrentMonster.AttackPoint;
            }
            else if (Board.NamekWorld == null)
            {
                EnterCity(CurrentMonster);
            }
            else if ((Jeu.Monsters.Count > 4) && (!Board.EarthWorldOccupied))
            {
                // Attack ennemy in namek
                Board.NamekWorld.Health -= CurrentMonster.AttackPoint;

                if (!Board.NamekWorld.IsAlive)
                {
                    Board.NamekWorld.Location = Location.Outside;
                    Board.NamekWorld = null;
                    EnterCity(CurrentMonster);
                }
                else
                {
                    EnterCity(CurrentMonster);
                    Attacked.Add(Board.NamekWorld);
                }
            }
            else
            {
                // Attack and check if NamekWorld not dead
                Board.NamekWorld.Health -= CurrentMonster.AttackPoint;

                if (!Board.NamekWorld.IsAlive)
                {
                    Board.NamekWorld.Location = Location.Outside;
                    Board.NamekWorld = null;
                    EnterCity(CurrentMonster);
                }
                else
                    Attacked.Add(Board.NamekWorld);

                // Attack and check if EarthWorld not dead
                if (Board.EarthWorld != null)
                {
                    Board.EarthWorld.Health -= CurrentMonster.AttackPoint;

                    if (!Board.EarthWorld.IsAlive)
                    {
                        Board.EarthWorld.Location = Location.Outside;
                        Board.EarthWorld = null;
                        EnterCity(CurrentMonster);
                    }
                    else
                        Attacked.Add(Board.EarthWorld);
                }
            }

            // After attack, set current monster attack pts to 0
            CurrentMonster.AttackPoint = 0;
        }

        public static void EndCard()
        {
            State = State.EndTurn;
        }

        public static void EndTurn()
        {
            // Check si un gagnant
            GetWinner();
            if(State == State.EndTurn)
                StartTurn();
        }

        public static void GetWinner()
        {
            int nbAlive = Array.FindAll(Monsters.ToArray(), (Monster m) => m.IsAlive).Length;

            // Find if only one is Alive
            if(nbAlive == 1)
            {
                State = State.EndGame;
                Winner = Array.Find(Monsters.ToArray(), (Monster m) => m.IsAlive);
            }

            // Check if someone has 20 victory points
            foreach(Monster m in Monsters)
            {
                if(m.VictoryPoint >= 20)
                {
                    State = State.EndGame;
                    Winner = m;
                }
            }
        }
    }
}
