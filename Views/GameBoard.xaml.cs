using KingOfTokyo.Controllers;
using KingOfTokyo.Models.Dices;
using KingOfTokyo.Models.Game;
using KingOfTokyo.Models.Monsters;
using KingOfTokyo.Models.Network;
using KingOfTokyo.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KingOfTokyo.Views
{
    /// <summary>
    /// Logique d'interaction pour GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Window
    {

        public bool IsOnline { get; set; }
        public int MyId { get; set; }

        private bool IsMe => Jeu.CurrentMonster == Jeu.Monsters[MyId];
        private bool IsMeOnline => IsOnline && IsMe;

        public GameBoard()
        {
            InitializeComponent();
            ClientController.WindowGame = this;
            HandleDiceButton();
        }

        public void SetMyId(int id)
        {
            MyId = id;
        }

        public void HandleDiceButton()
        {
            uc_Dice.btn_handle_roll.Click += new RoutedEventHandler(btn_dice_roll);
            for(int i = 0; i < uc_Dice.Buttons.Count; i++)
            {
                uc_Dice.Buttons[i].Click += new RoutedEventHandler(btn_dice_save);
            }
        }

        public void StartGame()
        {
            Jeu.StartGame();
            CreateCharacterList();

            HideCardTurn();
            HideDiceTurn();
            HideEndTurn();
            HideExitNamek();
            HideEndGame();
            HideCardsForSale();

            UpdateGame();
            showCards();
        }

        public void UpdateDiceView()
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                lbl_try_roll.Content = "Lancé restant " + Jeu.MaxRoll;
                uc_Dice.SetImages(DiceRoller.Dices);
            });
        }

        public void UpdateGame()
        {
            switch (Jeu.State)
            {
                case State.Roll:
                    SetDiceTurn();
                    break;
                case State.Card:
                    SetCardTurn();
                    break;
                case State.EndTurn:
                    SetEndTurn();
                    break;
                case State.ExitWorld:
                    SetExitNamek();
                    break;
                case State.EndGame:
                    SetEndGame();
                    break;

            }
        }
        public void CreateCharacterList()
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                wrap_monsters.Children.Clear();
                for (int i = 0; i < Jeu.Monsters.Count; i++)
                {
                    Monster m = Jeu.Monsters[i];
                    ucMonsterInfo uc = new ucMonsterInfo(m);
                    uc.Margin = new Thickness(10, 10, 10, 10);

                    if (!IsOnline)
                    {
                        if (m == Jeu.CurrentMonster)
                        {
                            
                            uc.Width += 30;
                            uc.Height += 30;
                        }
                        else
                        {
                            uc.Height -= 20;
                            uc.Width -= 20;
                        }
                    }
                    else
                    {
                        if(m.Id == MyId)
                        {
                            uc.BorderBrush = new SolidColorBrush(Colors.Red);
                            uc.BorderThickness = new Thickness(1, 1, 1, 1);

                            uc.Width += 30;
                            uc.Height += 30;
                        }
                        else
                        {
                            uc.Height -= 20;
                            uc.Width -= 20;
                        }
                    }
                    wrap_monsters.Children.Add(uc);
                }

                if (Board.NamekWorld != null)
                    img_monster_tokyo.Source = new BitmapImage(new Uri(@"/KingOfTokyo;component/Resources/face/face_" + Board.NamekWorld.Pseudo + ".png", UriKind.Relative));
                else
                    img_monster_tokyo.Source = null;

                if (Board.EarthWorld != null)
                    img_monster_tokyo_bay.Source = new BitmapImage(new Uri(@"/KingOfTokyo;component/Resources/face/face_" + Board.EarthWorld.Pseudo + ".png", UriKind.Relative));
                else
                    img_monster_tokyo_bay.Source = null;
            });
        }

        private void btn_dice_roll(object sender, RoutedEventArgs e)
        {
            if (Jeu.MaxRoll == 0)
            {
                MessageBox.Show("Vous n'avez plus de lancé");
                return;
            }
           
            if (IsOnline)
            {
                if (Jeu.CurrentMonster == Jeu.Monsters[MyId])
                {
                    Client.SendRequest(Command.Roll, "");
                }
            }
            else
            {
                Jeu.Roll();
                UpdateDiceView();
            }

        }

        private void btn_dice_save(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            // Dice index
            int id = int.Parse(button.Tag.ToString()) - 1;

            if (IsOnline)
                Client.SendRequest(Command.SaveDice, id.ToString());
            else
            {
                DiceRoller.Dices[id].Save = !DiceRoller.Dices[id].Save;
                uc_Dice.UpdateDice(id);
            }
        }

        private void btn_end_turn_Click(object sender, RoutedEventArgs e)
        {
            if (IsOnline)
                Client.SendRequest(Command.EndTurn, "");
            else
                EndTurn();
        }

        private void btn_end_roll_Click(object sender, RoutedEventArgs e)
        {
            if(Jeu.MaxRoll != 3)
            {
                if (IsOnline)
                    Client.SendRequest(Command.EndRoll, "");
                else
                {
                    Jeu.EndRoll();
                    CreateCharacterList();
                    UpdateGame();
                }
            }
        }

        private void btn_end_card_Click(object sender, RoutedEventArgs e)
        {
            if (IsOnline)
                Client.SendRequest(Command.EndCard, "");
            else
            {
                Jeu.EndCard();
                UpdateGame();
            }

        }

        private void btn_stay_city_Click(object sender, RoutedEventArgs e)
        {
            if (!IsOnline)
            {
                Jeu.ExitNamek(false);
                CreateCharacterList();
                UpdateGame();
            }
            else
            {
                Client.SendRequest(Command.StayNamek, "");
            }
        }

        private void btn_exit_city_Click(object sender, RoutedEventArgs e)
        {
            if (!IsOnline)
            {
                Jeu.ExitNamek(true);
                CreateCharacterList();
                UpdateGame();
            }
            else
            {
                Client.SendRequest(Command.ExitNamek, "");
            }
        }

        private void EndTurn()
        {
            if (IsOnline)
                Client.SendRequest(Command.EndTurn, "");
            else
            {
                Jeu.EndTurn();
                CreateCharacterList();
                UpdateGame();
            }
        }

        private void HideDiceTurn()
        {
            Dispatcher.Invoke((Action) delegate () {
                grid_dice.Visibility = Visibility.Hidden;
                uc_Dice.Visibility = Visibility.Hidden;
            });

        }
        private void HideCardTurn()
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                wrap_player_deck.Children.Clear();
                grid_card.Visibility = Visibility.Hidden;
                grid_btn_card.Visibility = Visibility.Hidden;
                wrap_player_deck.Visibility = Visibility.Hidden;
            });
        }

        private void HideEndTurn()
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                btn_end_turn.Visibility = Visibility.Hidden;
            });
        }

        private void HideExitNamek()
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                grid_exit_namek.Visibility = Visibility.Hidden;
                grid_btn_exit.Visibility = Visibility.Hidden;
            });
        }

        private void HideEndGame()
        {
            Dispatcher.Invoke((Action)delegate ()
           {
               grid_endgame.Visibility = Visibility.Hidden;
           });
        }

        private void SetDiceTurn()
        {
            uc_Dice.ClearImages();
            HideEndTurn();

            Dispatcher.Invoke((Action)delegate ()
            {
                // First state, set image turn 
                img_monster_turn.Source = new BitmapImage(new Uri(@"/KingOfTokyo;component/Resources/face/face_" + Jeu.CurrentMonster.Pseudo + ".png", UriKind.Relative));
                lbl_game_state.Text = "Au tour de " + Jeu.CurrentMonster.Pseudo + " ** Lancement des dés **";

                uc_Dice.Visibility = Visibility.Visible;

                if (!IsOnline)
                    grid_dice.Visibility = Visibility.Visible;
                else if (IsMeOnline)
                    grid_dice.Visibility = Visibility.Visible;

                lbl_try_roll.Content = "Lancé restant " + Jeu.MaxRoll;

            });
        }

        private void SetCardTurn()
        {
            HideDiceTurn();
            HideExitNamek();

            Dispatcher.Invoke((Action)delegate ()
            {
                lbl_game_state.Text = "Au tour de " + Jeu.CurrentMonster.Pseudo + " ** Achat/Utilisation des cartes **";

                btn_buy1.Visibility = Visibility.Visible;
                btn_buy2.Visibility = Visibility.Visible;
                btn_buy3.Visibility = Visibility.Visible;

                if (!IsOnline)
                {
                    grid_card.Visibility = Visibility.Visible;
                    grid_btn_card.Visibility = Visibility.Visible;
                    wrap_player_deck.Visibility = Visibility.Visible;
                    GenerateDeckPlayer();
                }
                else if (IsMeOnline){
                    grid_btn_card.Visibility = Visibility.Visible;
                    grid_card.Visibility = Visibility.Visible;
                    wrap_player_deck.Visibility = Visibility.Visible;
                }
                    
            });
        }

        public void GenerateDeckPlayer()
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                wrap_player_deck.Children.Clear();
                foreach (Card c in Jeu.CurrentMonster.Cards)
               {
                   Image image = new Image()
                   {
                       Width = 60,
                       Height = 90,
                       Stretch = Stretch.Fill,
                       Source = new BitmapImage(new Uri(@"/KingOfTokyo;component/Resources/cardgame/" + c.Id + ".png", UriKind.Relative))
                   };

                   wrap_player_deck.Children.Add(image);
               }
           });

        }

        private void SetEndTurn()
        {
            HideCardTurn();
            Dispatcher.Invoke((Action)delegate ()
            {
                lbl_game_state.Text = "Au tour de " + Jeu.CurrentMonster.Pseudo + " ** Fin du tour **";

                if (!IsOnline)
                    btn_end_turn.Visibility = Visibility.Visible;
                else if (IsMeOnline)
                    btn_end_turn.Visibility = Visibility.Visible;
            });
        }

        private void SetExitNamek()
        {
            HideDiceTurn();
            Dispatcher.Invoke((Action)delegate ()
            {
                grid_btn_exit.Visibility = Visibility.Hidden;
                lbl_game_state.Text = Jeu.CurrentMonster.Pseudo + " ** Attaque les mondes **";

                img_monster_attacked.Source = new BitmapImage(new Uri(@"/KingOfTokyo;component/Resources/face/face_" + Jeu.Attacked[0].Pseudo + ".png", UriKind.Relative));
                grid_exit_namek.Visibility = Visibility.Visible;

                if (!IsOnline)
                    grid_btn_exit.Visibility = Visibility.Visible;
                else if ((IsOnline) && (Jeu.Attacked[0].Id == MyId))
                    grid_btn_exit.Visibility = Visibility.Visible;
            });
        }

        private void SetEndGame()
        {
            HideCardTurn();
            HideDiceTurn();
            HideEndTurn();
            HideExitNamek();
            HideCardsForSale();

            Dispatcher.Invoke((Action)delegate ()
            {
                grid_turn.Visibility = Visibility.Hidden;
                img_endgame.Source = new BitmapImage(new Uri(@"/KingOfTokyo;component/Resources/face/face_" + Jeu.Winner.Pseudo + ".png", UriKind.Relative));
                grid_endgame.Visibility = Visibility.Visible;
            });
        }

        private void HideCardsForSale()
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                btn_buy1.Visibility = Visibility.Hidden;
                btn_buy2.Visibility = Visibility.Hidden;
                btn_buy3.Visibility = Visibility.Hidden;
            });
        }

        private void btn_endgame_Click(object sender, RoutedEventArgs e)
        {
            if (IsOnline)
            {
                MessageBox.Show("Merci d'avoir joué à King of Namek");
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            mainWindow.BringIntoView();
            Window.GetWindow(this).Close();

        }

        private void checkCardForSale(Image img, int index, Button btn)
        {
            Dispatcher.Invoke((Action) delegate()
            {
                try
                {
                    img.Source = new BitmapImage(new Uri(@"/KingOfTokyo;component/Resources/cardgame/" + Jeu.CardsForSale[index].Id + ".png", UriKind.Relative));
                }
                catch
                {
                    img.Visibility = Visibility.Hidden;
                    btn.Visibility = Visibility.Hidden;

                }
            });
        }

        public void showCards()
        {
            checkCardForSale(img_card1, 0, btn_buy1);
            checkCardForSale(img_card2, 1, btn_buy2);
            checkCardForSale(img_card3, 2, btn_buy3);
        }

        private void Btn_buy1_Click(object sender, RoutedEventArgs e)
        {
            if (!IsOnline)
            {
                if (Jeu.CurrentMonster.Energy < Jeu.CardsForSale[0].Price)
                    MessageBox.Show("Vous n'avez pas assez de points d'énergie");
                else
                    Jeu.BuyCard(0);
                showCards();
                CreateCharacterList();
                GenerateDeckPlayer();
            }
            if (IsMeOnline)
            {
                if (Jeu.CurrentMonster.Energy < Jeu.CardsForSale[0].Price)
                    MessageBox.Show("Vous n'avez pas assez de points d'énergie");
                else
                    Client.SendRequest(Command.BuyCard, "0");
            }

        }

        private void Btn_buy2_Click(object sender, RoutedEventArgs e)
        { 
            if (!IsOnline)
            {
                if (Jeu.CurrentMonster.Energy < Jeu.CardsForSale[1].Price)
                    MessageBox.Show("Vous n'avez pas assez de points d'énergie");
                else
                    Jeu.BuyCard(1);
                showCards();
                CreateCharacterList();
                GenerateDeckPlayer();
            }
            if (IsMeOnline)
            {
                if (Jeu.CurrentMonster.Energy < Jeu.CardsForSale[0].Price)
                    MessageBox.Show("Vous n'avez pas assez de points d'énergie");
                else
                    Client.SendRequest(Command.BuyCard, "1");
            }

        }

        private void Btn_buy3_Click(object sender, RoutedEventArgs e)
        {
            if(!IsOnline)
            {
                if (Jeu.CurrentMonster.Energy < Jeu.CardsForSale[2].Price)
                    MessageBox.Show("Vous n'avez pas assez de points d'énergie");
                else
                    Jeu.BuyCard(2);
                showCards();
                CreateCharacterList();
                GenerateDeckPlayer();
            }
            if (IsMeOnline)
            {
                if (Jeu.CurrentMonster.Energy < Jeu.CardsForSale[0].Price)
                    MessageBox.Show("Vous n'avez pas assez de points d'énergie");
                else
                    Client.SendRequest(Command.BuyCard, "2");
            }
        }

        private void img_card_MouseEnter(object sender, MouseEventArgs e)
        { 
            Image image = (Image)sender;
            img_zoom.Visibility = Visibility.Visible;
            img_zoom.Source = image.Source;

        }

        private void img_card_MouseLeave(object sender, MouseEventArgs e)
        {
            Image image = (Image)sender;
            img_zoom.Source = null;
            img_zoom.Visibility = Visibility.Hidden;
        }
    }
}
