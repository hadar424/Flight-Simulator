using FlightSimulator.ViewModels;
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

namespace FlightSimulator.Views
{
    /// <summary>
    /// Interaction logic for JoistickWin.xaml
    /// </summary>
    public partial class JoistickWin : UserControl
    {
        public JoistickWin()
        {
            InitializeComponent();
            DataContext = new ManualViewModel();
        }

        private void Joystick_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Joystick_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
