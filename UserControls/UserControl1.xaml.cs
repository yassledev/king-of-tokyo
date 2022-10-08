using KingOfTokyo.Models.Dices;
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
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {

        public List<Button> Buttons { get; set; } = new List<Button>();

        public UserControl1()
        {
            InitializeComponent();

            Buttons.Add(btn_dice_1);
            Buttons.Add(btn_dice_2);
            Buttons.Add(btn_dice_3);
            Buttons.Add(btn_dice_4);
            Buttons.Add(btn_dice_5);
            Buttons.Add(btn_dice_6);
        }

        public void ClearImages()
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                for (int i = 0; i < Buttons.Count; i++)
                {
                    Buttons[i].Content = "";
                    Buttons[i].BorderThickness = new Thickness(1, 1, 1, 1);
                }
            });
        }

        public void SetImages(List<Dice> dices)
        {
            for(int i = 0; i < dices.Count; i++)
            {
                Buttons[i].Content = new Image()
                {
                    Source = GetBitmapImage(dices[i]),
                    Stretch = Stretch.Fill
                };
            }
        }

        public void UpdateDice(int id)
        {
            Dispatcher.Invoke((Action)delegate ()
           {
               Dice dice = DiceRoller.Dices[id];

               if (dice.Save)
               {
                   Buttons[id].BorderThickness = new Thickness(5, 5, 5, 5);
                   Buttons[id].BorderBrush = new SolidColorBrush(Colors.Blue);
               }
               else
               {
                   Buttons[id].BorderThickness = new Thickness(1, 1, 1, 1);
                   Buttons[id].BorderBrush = new SolidColorBrush(Colors.Transparent);
               }
           });

        }

        public BitmapImage GetBitmapImage(Dice dice)
        {
            string nom = "";
            switch (dice.Symbol)
            {
                case (Symbol.Attack):
                    nom = "dice_attack";
                    break;
                case (Symbol.Energy):
                    nom = "dice_energy";
                    break;
                case (Symbol.Health):
                    nom = "dice_health";
                    break;
                case (Symbol.One):
                    nom = "dice_1";
                    break;
                case (Symbol.Two):
                    nom = "dice_2";
                    break;
                case (Symbol.Three):
                    nom = "dice_3";
                    break;
            }
            return new BitmapImage(new Uri(@"/KingOfTokyo;component/Resources/" + nom + ".png", UriKind.Relative));
        }
    }
}
