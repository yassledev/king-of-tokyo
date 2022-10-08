using KingOfTokyo.Models.Network;
using KingOfTokyo.Views;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace KingOfTokyo
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool Online;

        public MainWindow()
        {
            InitializeComponent();
            grid_panel_game.Visibility = Visibility.Hidden;
        }

        private void btn_solo_Click(object sender, RoutedEventArgs e)
        {
            Online = false;
            CharacterSelection characterSelection;
            characterSelection = new CharacterSelection(Online);
            characterSelection.Show();
            characterSelection.BringIntoView();
            Window.GetWindow(this).Close();
        }
        private void btn_online_Click(object sender, RoutedEventArgs e)
        {
            PlayOnline();
        }

        private void PlayOnline()
        {
            Online = true;
            grid_panel_game.Visibility = Visibility.Visible;
            txt_ip.Text = GetLocalIPAddress();
        }

        private void CheckServer()
        {

            int port;
            if(!int.TryParse(txt_port.Text, out port))
            {
                throw new Exception("Le port n'est pas un entier");
            }

            Dispatcher.Invoke((Action)delegate ()
            {
               try
               {

                   Client.Connect(txt_ip.Text, port);
                    Client.Disconnect();
                   btn_start.Content = "Rejoindre";
               }
               catch (Exception e)
               {
                   btn_start.Content = "Heberger";
                }
           });
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            CharacterSelection characterSelection;
            if (Online)
            {

                int port;
                if (!int.TryParse(txt_port.Text, out port))
                {
                    throw new Exception("Le port n'est pas un entier");
                }

                try
                {
                    Client.Connect(txt_ip.Text, port);
                }
                catch (Exception)
                {
                    Thread thread = new Thread(() => Serveur.Setup(port));
                    thread.Start();
                    Client.Connect(txt_ip.Text, port);
                }
                characterSelection = new CharacterSelection(true);
            }
            else
                characterSelection = new CharacterSelection(false);


            characterSelection.Show();
            characterSelection.BringIntoView();
            Window.GetWindow(this).Close();
        }

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

    }
}
