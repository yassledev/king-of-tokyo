using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using KingOfTokyo.Models.Monsters;
using KingOfTokyo.Models.Network;
using KingOfTokyo.Controllers;
using KingOfTokyo.Models.Game;
using KingOfTokyo.UserControls;
using System.Windows.Input;

namespace KingOfTokyo.Views
{
    /// <summary>
    /// Logique d'interaction pour CharacterSelection.xaml
    /// </summary>
    public partial class CharacterSelection : Window
    {
        // Online reference
        public bool IsOnline { get; set; }
        public int SelectedPlayer { get; set; }


        // Characters
        public List<string> Characters { get; set; } = new List<string> { "goku", "vegeta", "beerus", "friza", "hit", "black" };
        private List<ucCharacter> ucCharacters = new List<ucCharacter>();

        public CharacterSelection(bool online)
        {
            InitializeComponent();
            ClientController.WindowCharacter = this;
            Jeu.ClearGame();
            // Set online game
            IsOnline = online;
            // Generate characters panels
            GenerateCharacter();
            // Join game
            NewPlayer();
        }

        private void NewPlayer()
        {
            Random random = new Random();

            if (!IsOnline)
            {
                int id_character = 0;
                // Choose non selected character
                for (int i = 0; i < Characters.Count; i++)
                {
                    if (!Array.Exists(Jeu.Players.ToArray(), (Player p) => p.Pseudo == Characters[i]))
                    {
                        id_character = i;
                        break;
                    }
                }

                Player player = new Player(0, Characters[id_character]);
                player.Id = Jeu.Players.Count;

                // Add to game list
                Jeu.Players.Add(player);

                // UpdateView
                UpdateOneView(player.Id);

                // Set cursor to new player
                SetSelectedView(player.Id);
            }
            else
            {
                Client.SendRequest(Command.JoinGame, "");
            }
        }

        public void UpdateView()
        {
            this.Dispatcher.Invoke((Action)delegate ()
            {
                for (int i = 0; i < ucCharacters.Count; i++)
                {
                    if (i < Jeu.Players.Count)
                        ucCharacters[i].Player = Jeu.Players[i];
                    else
                        ucCharacters[i].Player = null;
                }
            });
        }

        public void UpdateOneView(int id)
        {
            this.Dispatcher.Invoke((Action) delegate ()
            {
                if(Array.Exists(Jeu.Players.ToArray(), (Player p) => p.Id == id))
                    ucCharacters[id].Player = Jeu.Players[id];
                else
                    ucCharacters[id].Player = null;  
            });
        }

        public void SetSelectedView(int id)
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                if (SelectedPlayer != id)
                    ucCharacters[SelectedPlayer].Selected = false;

                SelectedPlayer = id;
                ucCharacters[id].Selected = true;
            });
        }

        public void StartGame()
        {
            Dispatcher.Invoke((Action)delegate ()
            {
               GameBoard gameBoard = new GameBoard();
               gameBoard.IsOnline = IsOnline;
               gameBoard.SetMyId(SelectedPlayer);
               gameBoard.StartGame();
               gameBoard.Show();
               gameBoard.BringIntoView();
               Window.GetWindow(this).Close();
            });
        }

        private void btn_handle_player(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string monster = button.Tag.ToString();

            if(!Array.Exists(Jeu.Players.ToArray(), (Player p) => p.Pseudo == monster))
            {
                Jeu.Players[SelectedPlayer].Pseudo = monster;
                UpdateOneView(SelectedPlayer);
                if (IsOnline)
                    Client.SendObject(Command.UpdatePlayer, Jeu.Players[SelectedPlayer]);
            }
            else
            {
                MessageBox.Show("Un joueur possède déjà ce personnage");
            }
        }

        private void btn_play_Click(object sender, RoutedEventArgs e)
        {
            if(Jeu.Players.Count < 2)
            {
                MessageBox.Show("Vous devez être minimum deux joueurs pour commencer une partie");
                return;
            }

            if (IsOnline)
            {
                Client.SendRequest(Command.StartGame, "Start");
            }
            else
            {
                StartGame();
            }
        }

        private void selected_player(object sender, RoutedEventArgs e)
        {
            Border border = (Border)sender;
            int id = int.Parse(border.Tag.ToString());

            if(ucCharacters[id].Player != null)
            {
                SetSelectedView(id);
            }
        }

        private void GenerateCharacter()
        {
            ucCharacters.Add(uc_character_1);
            ucCharacters.Add(uc_character_2);
            ucCharacters.Add(uc_character_3);
            ucCharacters.Add(uc_character_4);
            ucCharacters.Add(uc_character_5);
            ucCharacters.Add(uc_character_6);

            if(!IsOnline)
            {
                for (int i = 0; i < ucCharacters.Count; i++)
                {
                    ucCharacters[i].uc_grid.MouseLeftButtonDown += new MouseButtonEventHandler(selected_player);
                    ucCharacters[i].uc_grid.Tag = i;
                }
                   
            }

            foreach (String s in Characters)
            {
                Button b = new Button();
                b.Content = new Image()
                {
                    Source = new BitmapImage(new Uri(@"/KingOfTokyo;component/Resources/face/face_" + s + ".png", UriKind.Relative)),
                    VerticalAlignment = VerticalAlignment.Center,
                    Stretch = Stretch.Fill,
                    Height = 90,
                    Width = 90,
                };
                b.Tag = s;
                b.Margin = new Thickness(10, 0, 10, 0);
                b.Click += new RoutedEventHandler(btn_handle_player);

                wrap_characters.Children.Add(b);
            }

            if (IsOnline)
            {
                btn_add_player.Visibility = Visibility.Hidden;
                btn_remove_player.Visibility = Visibility.Hidden;
            }
        }

        private void btn_add_player_Click(object sender, RoutedEventArgs e)
        {
            if(Jeu.Players.Count < 6)
                NewPlayer();
        }

        private void btn_remove_player_Click(object sender, RoutedEventArgs e)
        {
            if(Jeu.Players.Count > 1)
            { 
                int id = Jeu.Players.Count - 1;

                // Update Selected player to player before
                SetSelectedView(id - 1);

                // Remove player from game
                Jeu.Players.RemoveAt(id);

                // Update player view
                UpdateOneView(id);
                
            }
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            if (Client.IsConnected) {
                Client.SendRequest(Command.QuitGame, "");
                Client.Disconnect();
            }
                

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }
    }
}
