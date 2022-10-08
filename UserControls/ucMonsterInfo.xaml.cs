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
    /// Logique d'interaction pour ucMonsterInfo.xaml
    /// </summary>
    public partial class ucMonsterInfo : UserControl
    {
        private readonly Monster Monster;

        public ucMonsterInfo()
        {
            InitializeComponent();
        }

        public ucMonsterInfo(Monster m)
        {
            InitializeComponent();
            Monster = m;
            UpdateMonster(m);
        }

        public void UpdateMonster(Monster m)
        {
            img_bg.Source= new BitmapImage(new Uri(@"/KingOfTokyo;component/Resources/card/card_" + m.Pseudo + ".png", UriKind.Relative));
            lbl_attack.Content = m.AttackPoint;
            lbl_energy.Content = m.Energy;
            lbl_health.Content = m.Health;
            lbl_victory_point.Content = m.VictoryPoint;
            lbl_name.Content = m.Pseudo;

            if (!m.IsAlive)
            {
                grid_card.Opacity = 0.25;
                lbl_dead.Visibility = Visibility.Visible;
            }
        }
    }
}
