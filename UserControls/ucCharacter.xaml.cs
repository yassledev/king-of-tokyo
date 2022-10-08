using KingOfTokyo.Models.Monsters;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KingOfTokyo.UserControls
{
    /// <summary>
    /// Logique d'interaction pour ucCharacter.xaml
    /// </summary>
    public partial class ucCharacter : UserControl
    {
        private Player player;
        public Player Player {
            get { return player; }
            set
            {
                player = value;
                UpdateComponent();
            }
        }

        private bool selected;
        public bool Selected { 
            get { return selected; } 
            set { selected = value; UpdateSelected(); } }

        public ucCharacter()
        {
            InitializeComponent();

            if(Player == null)
            {
                lbl_pseudo.Visibility = Visibility.Hidden;
            }
        }

        private void UpdateSelected()
        {

            if (Selected)
            {
                uc_grid.BorderBrush = new SolidColorBrush(Colors.Blue);
                uc_grid.BorderThickness = new Thickness(5, 5, 5, 5);
            }
            else
            {
                uc_grid.BorderBrush = new SolidColorBrush(Colors.Transparent);
                uc_grid.BorderThickness = new Thickness(1, 1, 1, 1);
            }
        }

        private void UpdateComponent()
        {
            if(Player == null)
            {
                img_bg.Source = null;
                lbl_pseudo.Content = "";
                lbl_pseudo.Visibility = Visibility.Hidden;
                uc_grid.BorderBrush = null;
            } 
            else
            {
                lbl_pseudo.Content = Player.Pseudo;
                lbl_pseudo.Visibility = Visibility.Visible;
                img_bg.Source = new BitmapImage(new Uri(@"/KingOfTokyo;component/Resources/body/body_" + Player.Pseudo + ".png", UriKind.Relative));
                img_bg.Stretch = Stretch.Fill;
            }
        }
    }
}
